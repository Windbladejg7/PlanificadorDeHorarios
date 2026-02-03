using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using PlanificadorDeHorarios.Api.Common;

namespace PlanificadorDeHorarios.Api.Features
{
    public record GuardarHorarioRequest(int idUsuario, List<Horario> horarios);
    public class GuardarHorarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) 
        {
            app.MapPost("horario/guardar", async (GuardarHorarioRequest request, [FromServices] GuardarHorarioHandler handler) =>
            {
                var result = await handler.Handle(request);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            });
        }
    }

    public class GuardarHorarioHandler : IHandler 
    {
        IHorarioRepositorio _repositorio;

        public GuardarHorarioHandler(IHorarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<string>> Handle(GuardarHorarioRequest request)
        {
            try
            {
                await _repositorio.GuardarHorariosGenerados(request.idUsuario, request.horarios);
                return Result<string>.Success("Horarios guardados exitosamente");
            }catch(Exception ex)
            {   
                return Result<string>.Failure("Error al guardar horarios "+ex.Message);
            }
        }
    }
}
