using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<IUsuarioRepositorio>();

var app = builder.Build();
app.Run();