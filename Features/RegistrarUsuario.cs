using Microsoft.AspNetCore.Mvc;
using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Features
{
    public record RegistrarUsuarioRequest(string nombre, string email, string password);

    public class RegistrarUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", async (RegistrarUsuarioRequest request, [FromServices] RegistrarUsuarioHandler handler) =>
            {
                var result = await handler.Handle(request);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.Conflict(new {message = result.Error});
            });
        }
    }

    public class RegistrarUsuarioHandler : IHandler
    {
        private IUsuarioRepositorio _repositorio;
        private IPasswordHelper _passwordHelper;

        public RegistrarUsuarioHandler(IUsuarioRepositorio repositorio, IPasswordHelper passwordHelper)
        {
            _repositorio = repositorio;
            _passwordHelper = passwordHelper;
        }

        public async Task<Result<string>> Handle(RegistrarUsuarioRequest request) 
        {
            bool usuarioExiste = await _repositorio.VerificarSiExistePorEmailAsync(request.email);
            if (usuarioExiste)
                return Result<string>.Failure("El usuario ya se encuentra registrado");

            string password = _passwordHelper.HashearPassword(request.password);
            Usuario usuario = new()
            {
                Nombre = request.nombre,
                Email = request.email,
                HashPassword = password
            };
            await _repositorio.AgregarUsuarioAsync(usuario);

            return Result<string>.Success("Usuario registrado exitosamente");
        }
    }
}
