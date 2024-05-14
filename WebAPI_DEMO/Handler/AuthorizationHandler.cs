// <copyright file="JwtMiddleware.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Platinum.WebApi.Home3D.Handler
{
    public class AuthorizationHandler
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public AuthorizationHandler(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var isAuthorized = await AttachAccountToContext(context, token);
                if (!isAuthorized)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var customResponse = new
                    {
                        status = 401,
                        message = "Unauthorized. Please Provide Valid Credentials"
                    };

                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(customResponse));
                    return; 
                }
            }

            await _next(context);
        }

        private async Task<bool> AttachAccountToContext(HttpContext context, string token)
        {
            try
            {
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

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var user = await _context.Users.SingleOrDefaultAsync(x => x.Id.ToString() == userId);
                    context.Items["User"] = user;

                    if (user != null)
                    {
                        var roles = await _context.Users
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .ThenInclude(x => x.RolePermissions)
                            .ThenInclude(x => x.Permission)
                            .Where(x => x.Id.ToString() == userId)
                            .SelectMany(x => x.UserRoles.Select(ur => ur.Role))
                            .ToListAsync();

                        context.Items["Roles"] = roles;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
