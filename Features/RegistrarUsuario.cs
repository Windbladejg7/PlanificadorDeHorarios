using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Common;

namespace PlanificadorDeHorarios.Api.Features
{
    record RegistrarUsuarioRequest(string nombre, string email, string password);

    public class RegistrarUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/register", (RegistrarUsuarioRequest request, IUsuarioRepositorio usuarioRepositorio, IPasswordHelper passwordHelper) =>
            {
                if (usuarioRepositorio.VerificarSiExistePorEmail(request.email))
                    return Results.Conflict(new{message = "El usuario ya existe"});

                string password = passwordHelper.HashearPassword(request.password);
                Usuario usuario = new()
                {
                    Nombre = request.nombre,
                    Email = request.email,
                    HashPassword = password
                };
                usuarioRepositorio.AgregarUsuario(usuario);
                return Results.Ok();
            });
        }
    }
}
