using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Lancelittle.Data;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.CharacterCantrip;
using Lancelittle.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lancelittle.Services.CharacterCantripService
{
    public class CharacterCantripService : ICharacterCantripService
    {
        //Mapper for using DTOs
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        //HttpContextAccessor allows us to access current user via claims
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterCantripService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;  
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterCantrip(AddCharacterCantripDto newCharacterCantrip)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                //Find the character including relic and all cantrips via character and user Id
                Character character = await _context.Characters
                    .Include(c => c.Relic)
                    .Include(c => c.CharacterCantrips).ThenInclude(cc => cc.Cantrip)                    
                    .FirstOrDefaultAsync(c => c.Id == newCharacterCantrip.CharacterId &&
                    c.User.Id == GetUserId());
                if(character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Operation Failed. Character Not Found.";
                    return serviceResponse;
                }
                //Find the cantrip via Id
                Cantrip cantrip = await _context.Cantrips
                    .FirstOrDefaultAsync(c => c.Id == newCharacterCantrip.CantripId);
                if(cantrip == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Operation Failed. Cantrip Not Found.";
                    return serviceResponse;
                }
                //Set up the relationship
                CharacterCantrip characterCantrip = new CharacterCantrip
                {
                    Character = character,
                    Cantrip = cantrip
                };
                //Add to DB
                await _context.CharacterCantrips.AddAsync(characterCantrip);
                //Always save
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}