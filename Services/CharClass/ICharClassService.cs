using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.CharClass;
using Lancelittle.Models;

namespace Lancelittle.Services.CharClassService
{
    public interface ICharClassService
    {
        public Task<ServiceResponse<GetCharacterDto>> SetClass(SetCharClassDto setClass);
    }
}