using System.Threading.Tasks;
using Lancelittle.DTOs.CharClass;
using Lancelittle.Services.CharClassService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lancelittle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharClassController : ControllerBase
    {
        private readonly ICharClassService _charClassService;

        public CharClassController(ICharClassService charClassService)
        {
            _charClassService = charClassService;
        }
        [HttpPut("SetClass")]
        public async Task<IActionResult> SetClass(SetCharClassDto setClass)
        {
            return Ok(await _charClassService.SetClass(setClass));
        }
    }
}