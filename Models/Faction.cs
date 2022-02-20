using System.Collections.Generic;

namespace Lancelittle.Models
{
    public class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Character> Characters { get; set; }
    }
}
