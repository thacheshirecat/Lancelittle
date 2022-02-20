using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.Relic;
using Lancelittle.Services.RelicService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lancelittle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RelicController : ControllerBase
    {
        private readonly IRelicService _relicService;

        public RelicController(IRelicService relicService)
        {
            _relicService = relicService;
        }
        [HttpPost]
        public async Task<IActionResult> AddRelic(AddRelicDto newRelic)
        {
            return Ok(await _relicService.AddRelic(newRelic));
        }
    }
}