using Microsoft.IdentityModel.JsonWebTokens;
using PlanificadorDeHorarios.Api.Ports;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Common;

namespace PlanificadorDeHorarios.Api.Features
{
    public class ObtenerHorariosGuardadosEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/horarios", async (ClaimsPrincipal usuario, [FromServices] ObtenerHorariosGuardadosHandler handler) =>
            {
                int idUsuario = int.Parse(usuario.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var respuesta = await handler.Handle(idUsuario);
                return respuesta.IsSuccess ? Results.Ok(respuesta.Value) : Results.BadRequest(respuesta.Error);

            }).RequireAuthorization();
        }
    }

    public class ObtenerHorariosGuardadosHandler : IHandler 
    {
        private IHorarioRepositorio _repositorio;

        public ObtenerHorariosGuardadosHandler(IHorarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<List<List<Horario>>>> Handle(int idUsuario)
        {
            try
            {
                var result = await _repositorio.ObtenerHorariosGuardados(idUsuario);
                return Result<List<List<Horario>>>.Success(result);
            }
            catch(Exception ex)
            {
                return Result<List<List<Horario>>>.Failure("Error la cargar los horarios " + ex.Message);
            }
        }
    }
}
