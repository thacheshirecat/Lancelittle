using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Lancelittle.Data;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.CharClass;
using Lancelittle.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lancelittle.Services.CharClassService
{
    public class CharClassService : ICharClassService
    {
        //Mapper for using DTOs
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        //HttpContextAccessor allows us to access current user via claims
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharClassService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;  
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCharacterDto>> SetClass(SetCharClassDto setClass)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                    .Include(c => c.Relic)
                    .Include(c => c.Class)
                    .Include(c => c.CharacterCantrips).ThenInclude(cc => cc.Cantrip)                    
                    .FirstOrDefaultAsync(c => c.Id == setClass.CharacterId &&
                    c.User.Id == GetUserId());
                if(character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Operation Failed. Character Not Found.";
                    return serviceResponse;
                }
                CharClass foundClass = await _context.Classes.FirstOrDefaultAsync(c => c.Id == setClass.CharClassId);
                if(foundClass == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Operation Failed. Class Not Found.";
                    return serviceResponse;
                }
                character.Class = foundClass;
                // foundClass.Characters.Add(character);
                // _context.Classes.Update(foundClass);
                _context.Characters.Update(character);
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