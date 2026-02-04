using PlanificadorDeHorarios.Api.Extensions;
using PlanificadorDeHorarios.Api.Infraestructure;
using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IHorarioRepositorio, HorarioRepositorio>();
builder.Services.AddHandlers();
builder.Services.AddScoped<IOcrApiAdapter, DocumentAiApiAdapter>();
builder.Services.AddScoped<GeneradorDeHorarios>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secreto)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
    });

builder.Services.AddCors(options =>{
    options.AddPolicy("PlanificadorPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("PlanificadorPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.Run();