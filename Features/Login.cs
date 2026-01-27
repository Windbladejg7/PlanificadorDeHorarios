using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Features
{
    public record LoginRequest(string email, string password);
    public class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/login", async (LoginRequest request, LoginHandler handler)=>
            {
                var result = await handler.Handle(request);

                return result.IsSuccess ? Results.Ok(new { token = result.Value }) : Results.Unauthorized();
            });
        }
    }

    public class LoginHandler : IHandler
    { 
        private IUsuarioRepositorio _repositorio;
        private IPasswordHelper _passwordHelper;
        private ITokenGenerator _tokenGenerator;

        public LoginHandler(IUsuarioRepositorio repositorio, IPasswordHelper passwordHelper, ITokenGenerator tokenGenerator)
        {
            _repositorio = repositorio;
            _passwordHelper = passwordHelper;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<string>> Handle(LoginRequest request)
        {
            Usuario usuario = await _repositorio.BuscarPorEmailAsync(request.email);

            if (usuario == null)
                return Result<string>.Failure("Usuario no existe");

            if (!_passwordHelper.VerificarPassword(request.password, usuario.HashPassword))
                return Result<string>.Failure("Contraseña incorrecta");

            return Result<string>.Success(_tokenGenerator.GenerarToken(usuario));
        }
    }
}
