using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.Models;
using Lancelittle.Services.CharacterService;
using Lancelittle.DTOs.Character;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Lancelittle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        //Dependency Injection in the constructor
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        //Three Get methods, one for All Characters, on for Characters by Id and one for Characters of a specific User
        
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _characterService.GetAllCharacters());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }
        [HttpGet("GetOwned")]
        public async Task<IActionResult> GetOwned()
        {
            //GetOwnedCharacters() method contains logic to find the current User Id
            return Ok(await _characterService.GetOwnedCharacters());
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCharacter(AddCharacterDto character)
        {
            return Ok(await _characterService.AddCharacter(character));
        }
        //Seperate out the admin route from the User route for Update: Most likely redundant, logic should be in single service method
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = await _characterService.UpdateCharacter(updatedCharacter);
            if(serviceResponse.Data == null)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }
        [HttpPut("updateowned")]
        public async Task<IActionResult> UpdateOwnedCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = await _characterService.UpdateOwnedCharacter(updatedCharacter);
            if(serviceResponse.Data == null)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }
        //Seperate out the admin route from the User route for Delete: Most likely redundant, logic should be in single service method
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = await _characterService.DeleteCharacter(id);
            //Data could be null because in our Delete method, if no matching Id is found an exception will be thrown and no data will be returned
            if(serviceResponse.Data == null)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }
    }
}