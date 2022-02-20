using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.DTOs.Skill;

namespace Lancelittle.DTOs.CharClass
{
    public class GetCharClassDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GetSkillDto> Skills { get; set; }
    }
}