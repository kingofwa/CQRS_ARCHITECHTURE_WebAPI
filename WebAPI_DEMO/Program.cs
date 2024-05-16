using Application;
using Application.Interfaces;
using Persistence;
using Platinum.WebApi.Home3D.Handler;
using WebAPI_Demo.Extensions;
using static WebAPI_Demo.Extensions.ConfigureSwaggerOptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add IHttpContextAccessor to the service container
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerWithVersioning();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<RemoveVersionParameterFilter>();
    options.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
});
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.UseSwaggerWithVersioning();
app.UseMiddleware<AuthorizationHandler>();
app.UseMiddleware<GlobalErrorHandler>();
app.MapControllers();

app.Run();
