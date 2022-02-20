using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lancelittle.DTOs.Relic
{
    public class AddRelicDto
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int CharacterId { get; set; }
    }
}