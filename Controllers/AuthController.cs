using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.Data;
using Lancelittle.DTOs.User;
using Lancelittle.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lancelittle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;            
        }
        //Method for registering new users
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto newUser)
        {
            //Use the Register Method we created to add a new user with the entered UserName and Password
            ServiceResponse<int> serviceResponse = await _authRepo.Register(
                new User { UserName = newUser.UserName }, newUser.Password
            );
            //Register method checks for duplicate users, and marks Success as a failure if found.
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }
        //Login Method
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            ServiceResponse<string> serviceResponse = await _authRepo.Login(user.UserName, user.Password);
            //The Login method will return Success false if the password or username is not successfully verified
            if(!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }
    }
}