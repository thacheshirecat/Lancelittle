using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lancelittle.DTOs.CharacterCantrip
{
    public class AddCharacterCantripDto
    {
        public int CharacterId { get; set; }
        public int CantripId { get; set; }
    }
}