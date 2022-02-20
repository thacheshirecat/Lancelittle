using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.CharClass;
using Lancelittle.Models;

namespace Lancelittle.DTOs.Character
{
    public class UpdateCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Defense { get; set; }
        public GetCharClassDto Class { get; set; }
        public Faction Faction { get; set; }
    }
}