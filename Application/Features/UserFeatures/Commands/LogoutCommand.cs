using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class LogoutCommand : IRequest<bool>
    {
        public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ITokenBlacklistService _tokenBlacklistService;

            public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor, ITokenBlacklistService tokenBlacklistService)
            {
                _httpContextAccessor = httpContextAccessor;
                _tokenBlacklistService = tokenBlacklistService;
            }

            public async Task<bool> Handle(LogoutCommand command, CancellationToken cancellationToken)
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    if (jwtToken != null)
                    {
                        await _tokenBlacklistService.AddTokenToBlacklist(token);
                        return true;
                    }
                }

                return false;
            }
        }
    }


}
