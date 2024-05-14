using Application.Dtos;
using Application.Features.ProductFeatures.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;
using Application.BaseResult;
using Microsoft.Extensions.Options;

namespace Application.Features.UserFeatures.Commands
{
    public class LoginCommand : IRequest<BaseResult<UserAccessInfoDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class LoginCommandCommandHandler : IRequestHandler<LoginCommand, BaseResult<UserAccessInfoDto>>
        {
            private readonly IApplicationDbContext _context;
            public LoginCommandCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<BaseResult<UserAccessInfoDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(x => x.UserRoles).ThenInclude(x =>x.Role)
                    .SingleOrDefaultAsync(x => x.Username == command.UserName);
                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);
                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        var token = GenerateJwtToken(user);
                        var result = new UserAccessInfoDto()
                        {
                            AccessToken = token,
                            Id = user.Id,
                            Role = user.UserRoles.FirstOrDefault()?.Role.Name
                        };

                        return new BaseResult<UserAccessInfoDto>()
                        {
                            Data = result,
                            Success  = true
                        };
                    }
                    else
                    {
                        throw new Exception("Invalid password");
                    }

                }
                throw new Exception("Invalid User");
            }

            private string GenerateJwtToken(User user)
            {
                var claims = new[]
                {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C1CF4B7DC4C4175B6618DE4F55CA4AAA"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: null,
                    audience: "LeUseLoginOrLogour",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }

}
