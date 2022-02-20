using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Lancelittle.Data;
using Lancelittle.DTOs.Character;
using Lancelittle.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lancelittle.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {        
        //Mapper for using DTOs
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        //HttpContextAccessor allows us to access current user via claims
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }
        //Since the User Id may be needed often, we create a method to retreive the User Id from the claims via Http Context
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private int GetAuthLevel() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role));

        //Regarding the return type: Task relates to the method being async, this requires the Task return type
        //ServiceResponse is a model we created to wrap the response into three catagories: Data, Success, and Message
        //List is a C# list, and GetCharacterDto is the DTO we want to use in this method for the model we created
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            //In each method, we first set up our serviceResponse to match the return type of the method
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //Here the data from the body is coming in as an AddCharacterDto, so we need to map it back to a Character for storage
            Character character = _mapper.Map<Character>(newCharacter);
            //Next, we need to set the character's user, we retreive this info from the claims with our GetUserId Method
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            //Call to the DataBase to add the incoming Data .AddAsync() Entity method to add entry into DB
            await _context.Characters.AddAsync(character);
            //Must save changes
            await _context.SaveChangesAsync();
            //In order to map an entire list, we need to use .Select with a lambda, and map each Character individually, then use .ToList() to create a list
            serviceResponse.Data = (_context.Characters.Where(c => c.User.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try {
                Character character = new Character();
                if(GetAuthLevel() == 1) {
                    character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id);
                }
                else {
                    //We prviously used .FirstAsync() here because if nothing is found it will throw an exception
                    character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                }
                //However, now we use an if statement to check the results
                if(character != null) {
                    //.Remove() Entity method to delete from DB
                    _context.Characters.Remove(character);
                    //Always save changes
                    await _context.SaveChangesAsync();
                    if(GetAuthLevel() == 1) {
                        serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
                    }
                    else {
                        //In order to map an entire list, we need to use .Select with a lambda, and map each Character individually, then use .ToList() to create a list
                        serviceResponse.Data = (_context.Characters.Where(c => c.User.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
                    }
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found.";
                }
            }
            catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            } 
            return serviceResponse;
        }
        //Method to return all Characters listed in the DB
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //Generate List of Characters using call to the Database with await and .ToListAsync()
            List<Character> dbCharacters = await _context.Characters
                .Include(c => c.Relic)
                .Include(c => c.Class)
                .Include(c => c.CharacterCantrips).ThenInclude(cc => cc.Cantrip)
                .ToListAsync();
            //In order to map an entire list to our desired DTO, we need to use .Select with a lambda
            //We then map each Character individually, then use .ToList() to regenerate the list
            serviceResponse.Data = (dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceResponse;
        }
        //This method allows us to retreive any character given their Id
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            //Call to DB looking for matching Character and User Id
            Character character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            //Here we map our Character data to our GetCharacterDto by using the injected AutoMapper
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetOwnedCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //Here we make use of .Where function in order to filter characters using the User Id from our GetUserId method
            List<Character> dbCharacters = await _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .Include(c => c.Relic)
                .ToListAsync();
            serviceResponse.Data = (dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceResponse;
        }
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

            try {
            //First, find the matching character on the DB via the provided Id
            Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            //Update each property individually
            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Dexterity = updatedCharacter.Dexterity;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Defense = updatedCharacter.Defense;
            //.Update() Entity method to update a DB entry
            _context.Characters.Update(character);
            //Always save changes
            await _context.SaveChangesAsync();
            //Map our response to a GetCharacterDto since it is a Character
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<GetCharacterDto>> UpdateOwnedCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

            try {
                //First, find the matching character on the DB via the provided Id
                //Include is important where working with relations. Without Including, Entity will return null for User or any other related objects
                Character character = await _context.Characters
                .Include(c => c.User)
                .Include(c => c.Relic)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if(character.User.Id == GetUserId())
                {
                    //Update each property individually
                    character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Strength = updatedCharacter.Strength;
                    character.Dexterity = updatedCharacter.Dexterity;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Defense = updatedCharacter.Defense;
                    //.Update() Entity method to update a DB entry
                    _context.Characters.Update(character);
                    //Always save changes
                    await _context.SaveChangesAsync();
                    //Map our response to a GetCharacterDto since it is a Character
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found.";
                }
            }
            catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}