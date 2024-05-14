using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Queries
{
    public class GetmeQuery : IRequest<UserInfoDto>
    {
        public class GetmeQueryHandler : IRequestHandler<GetmeQuery, UserInfoDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IConfiguration _configuration;
            public GetmeQueryHandler(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context , IConfiguration configuration)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
                _configuration = configuration;
            }

            public async Task<UserInfoDto> Handle(GetmeQuery command, CancellationToken cancellationToken)
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWTSettings:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "uid").Value;

                var user = await _context.Users
                    .Include(x => x.UserRoles).ThenInclude(x => x.Role).ThenInclude(x => x.RolePermissions).ThenInclude(x => x.Permission)
                    .SingleOrDefaultAsync(x => x.Id.ToString() == userId);

                if (user != null)
                {
                    var allPermissions = user.UserRoles
                        .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name))
                        .Distinct()
                        .ToList();
                    var result = new UserInfoDto()
                    {
                        Name = user.Username,
                        Permissions = allPermissions,
                        UserRole = user.UserRoles.FirstOrDefault()?.Role.Name
                    };

                    return result;
                }
                throw new Exception("User not found");
            }
        }
    }
}
