using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.CharacterCantrip;
using Lancelittle.Models;

namespace Lancelittle.Services.CharacterCantripService
{
    public interface ICharacterCantripService
    {
        Task<ServiceResponse<GetCharacterDto>> AddCharacterCantrip(AddCharacterCantripDto newCharacterCantrip);
    }
}