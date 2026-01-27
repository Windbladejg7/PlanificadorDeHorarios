using PlanificadorDeHorarios.Api.Extensions;
using PlanificadorDeHorarios.Api.Infraestructure;
using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IUsuarioRepositorio>();
builder.Services.AddHandlers();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();
app.Run();