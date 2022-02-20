using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Lancelittle.Data;
using Lancelittle.DTOs.Character;
using Lancelittle.DTOs.Relic;
using Lancelittle.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lancelittle.Services.RelicService
{
    public class RelicService : IRelicService
    {
        //Mapper for using DTOs
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        //HttpContextAccessor allows us to access current user via claims
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RelicService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;  
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCharacterDto>> AddRelic(AddRelicDto newRelic)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newRelic.CharacterId &&
                    c.User.Id == GetUserId());
                if(character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Operation Failed. Character Not Found.";
                    return serviceResponse;
                }
                Relic relic = new Relic
                {
                    Name = newRelic.Name,
                    Power = newRelic.Power,
                    Character = character
                };
                await _context.Relics.AddAsync(relic);
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