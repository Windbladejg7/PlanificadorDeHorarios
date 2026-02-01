using PlanificadorDeHorarios.Api.Extensions;
using PlanificadorDeHorarios.Api.Infraestructure;
using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddHandlers();
builder.Services.AddScoped<IOcrApiAdapter, DocumentAiApiAdapter>();
builder.Services.AddScoped<GeneradorDeHorarios>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

app.MapEndpoints();
app.Run();