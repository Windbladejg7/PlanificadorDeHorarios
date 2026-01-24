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
            app.MapGet("/register", (RegistrarUsuarioRequest request, RegistrarUsuarioHandler handler) =>
            {
                var result = handler.Handle(request);

                return result.IsSuccess ? Results.Ok() : Results.Conflict(new {message = result.Error});
            });
        }
    }

    public class RegistrarUsuarioHandler 
    {
        private IUsuarioRepositorio _repositorio;
        private IPasswordHelper _passwordHelper;

        public RegistrarUsuarioHandler(IUsuarioRepositorio repositorio, IPasswordHelper passwordHelper)
        {
            _repositorio = repositorio;
            _passwordHelper = passwordHelper;
        }

        public Result<string> Handle(RegistrarUsuarioRequest request) 
        {
            if (_repositorio.VerificarSiExistePorEmail(request.email))
                return Result<string>.Failure("El usuario ya se encuentra registrado");

            string password = _passwordHelper.HashearPassword(request.password);
            Usuario usuario = new()
            {
                Nombre = request.nombre,
                Email = request.email,
                HashPassword = password
            };
            _repositorio.AgregarUsuario(usuario);

            return Result<string>.Success("Usuario registrado exitosamente");
        }
    }
}
