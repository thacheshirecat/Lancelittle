using System.Collections.Generic;

namespace Lancelittle.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Adventurer";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Defense { get; set; } = 0;
        public CharClass Class { get; set; }
        public Faction Faction { get; set; }
        public Relic Relic { get; set; }
        public User User { get; set; }
        public List<CharacterCantrip> CharacterCantrips { get; set; }
    }
}