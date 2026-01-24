using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Common;
using Microsoft.AspNetCore.Identity;

namespace PlanificadorDeHorarios.Api.Features
{
    public record RegistrarUsuarioRequest
    {
        public required string nombre;
        public required string email;
        public required string password;
    }

    public class RegistrarUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/register", (RegistrarUsuarioRequest request, IUsuarioRepositorio usuarioRepositorio, IPasswordHelper passwordHelper) =>

            {
                if (usuarioRepositorio.VerificarSiExistePorEmail(request.email)) return;

                string password = passwordHelper.HashearPassword(request.password);
                Usuario usuario = new()
                {
                    Nombre = request.nombre,
                    Email = request.email,
                    HashPassword = password
                };
                usuarioRepositorio.AgregarUsuario(usuario);
            });
        }
    }
}
