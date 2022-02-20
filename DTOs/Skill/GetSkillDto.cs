using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lancelittle.DTOs.Skill
{
    public class GetSkillDto
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Cost { get; set; }
    }
}