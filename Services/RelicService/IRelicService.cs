using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.Relic;
using Lancelittle.Models;

namespace Lancelittle.Services.RelicService
{
    public interface IRelicService
    {
        Task<ServiceResponse<GetCharacterDto>> AddRelic(AddRelicDto newRelic);
    }
}