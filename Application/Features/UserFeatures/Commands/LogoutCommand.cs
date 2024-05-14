using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using DotNetOpenAuth.OAuth.ChannelElements;
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
            public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
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
                        var expirationTime = jwtToken.ValidTo;

                        if (expirationTime > DateTime.UtcNow)
                        {
                            var updatedToken = new JwtSecurityToken(jwtToken.Issuer, null, jwtToken.Claims, DateTime.UtcNow.AddDays(-7), jwtToken.ValidTo, jwtToken.SigningCredentials);

                            var tokenString = tokenHandler.WriteToken(updatedToken);

                            return true; 
                        }
                    }
                }

                return false; 
            }
        }
    }
}
