using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lancelittle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Lancelittle.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        //We add the IConfiguration to the contructor so we can access our secret key used in creating JWTokens
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            User foundUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username.ToLower()));
            //If nothing is found when querying the DB, success is false and we can set an error message
            if(foundUser == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User Not Found.";
            //If we do find the user on the DB via username, we next need to check the password using the VerifyPassowordHash method we created
            } else if(!VerifyPasswordHash(password, foundUser.PasswordHash, foundUser.PasswordSalt)) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Wrong Password.";
            } else {
                serviceResponse.Data = CreateToken(foundUser);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();
            if(await UserExists(user.UserName))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User already exists.";
                return serviceResponse;
            }
            //When using methods with out parameters, define them as below so they can be used elsewhere in following code.
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            //Add the Salt and Hash to the user object
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            //Add the User to the DB
            await _context.Users.AddAsync(user);
            //Always save changes
            await _context.SaveChangesAsync();
            
            serviceResponse.Data = user.Id;

            return serviceResponse;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(c => c.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
        //The out keyword allows us to pass data up without having a return value for the method
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //Using the HMAC automatically generates a key which can be used as the salt
                passwordSalt = hmac.Key;
                //HMAC method .ComputeHash() creates the hashed password
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        //This method is used to verify the password hash that was created upon registration
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //Here, we use the HMAC, however this time we pass in the passwordSalt saved to the DB.. 
            //..in order to set the HMAC Key we will be using when we compute the hash for comparison
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //We use a simple for loop to compare what is saved on the DB with the hashed password computed with the stored Salt
                //Because this value should be 1 to 1, we can just check each position of the byte arrays. If any don't match, the entire password is wrong
                for(int i = 0; i < computedHash.Length; i++) {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        //This method allows us to create a JWToken for the user session
        private string CreateToken(User user)
        {
            //Claims can be sent along with the Token to provide additional information, such as the username, Id, or authorization level
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.AuthLevel.ToString())
            };

            //We generate the secret key from _configuration
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );
            //We create SigningCredentials using the key we generated, and the HMACSHA512 signature as creds
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //We bundle the claims, expiration date, and credentials into a Token Descriptor
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //User UtcNow to get correct time values regardless of where the user is located.
                Expires = DateTime.UtcNow.AddHours(1.0),
                SigningCredentials = creds
            };
            //This two step process is where we create the actual Token
            //First create the token handler, which has the .CreateToken() method that takes a Token Descriptor as it's parameter
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            //Then create the token
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //JwtSecurityTokenHandler also has the method .WriteToken() which can produce the token as a string
            return tokenHandler.WriteToken(token);
        }
    }
}