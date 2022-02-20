using System.Collections.Generic;

namespace Lancelittle.Models
{
    public class Cantrip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public List<CharacterCantrip> CharacterCantrips { get; set; }
    }
}