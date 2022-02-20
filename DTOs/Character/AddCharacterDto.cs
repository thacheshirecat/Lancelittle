using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.CharClass;
using Lancelittle.Models;

namespace Lancelittle.DTOs.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = "Adventurer";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Defense { get; set; } = 0;
        public GetCharClassDto Class { get; set; }
        public Faction Faction { get; set; }
    }
}