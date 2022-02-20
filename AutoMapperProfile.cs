using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lancelittle.Models;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.Relic;
using Lancelittle.DTOs.Cantrip;
using Lancelittle.DTOs.Skill;
using Lancelittle.DTOs.CharClass;

namespace Lancelittle
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //When creating a mapping profile, the First Parameter is the model we have, while the Second Parameter is the one we want to map it to
            CreateMap<Character, GetCharacterDto>()
                .ForMember(dto => dto.Cantrips, c => c.MapFrom(c => c.CharacterCantrips.Select(cc => cc.Cantrip)));
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, GetCharacterDto>();
            CreateMap<Relic, GetRelicDto>();
            CreateMap<Cantrip, GetCantripDto>();
            CreateMap<CharClass, GetCharClassDto>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}