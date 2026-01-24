using PlanificadorDeHorarios.Api.Extensions;
using PlanificadorDeHorarios.Api.Infraestructure;
using PlanificadorDeHorarios.Api.Ports;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IUsuarioRepositorio>();
builder.Services.AddHandlers();

var app = builder.Build();
app.Run();