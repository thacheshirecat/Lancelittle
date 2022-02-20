using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.CharacterCantrip;
using Lancelittle.Services.CharacterCantripService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lancelittle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterCantripController : ControllerBase
    {
        private readonly ICharacterCantripService _characterCantripService;

        public CharacterCantripController(ICharacterCantripService characterCantripService)
        {
            _characterCantripService = characterCantripService;
        }
        public async Task<IActionResult> AddCharacterCantrip(AddCharacterCantripDto newCantrip)
        {
            return Ok(await _characterCantripService.AddCharacterCantrip(newCantrip));
        }
    }
}