using Microsoft.AspNetCore.Mvc;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Features
{
    public record GenerarHorariosRequest(List<Materia> materias);
    public class GenerarHorariosEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/horarios", (GenerarHorariosRequest request, [FromServices] GenerarHorarioHandler handler) =>
            {
                try
                {
                    var response = handler.Handle(request);
                    return Results.Ok(response);
                }catch(Exception ex)
                {
                    return Results.InternalServerError();
                }
            });
        }
    }

    public class GenerarHorarioHandler : IHandler
    {
        private GeneradorDeHorarios _generador;

        public GenerarHorarioHandler(GeneradorDeHorarios generador) 
        {
            this._generador = generador;
        }

        public List<Horario> Handle(GenerarHorariosRequest request)
        {
            return _generador.generar(request.materias);
        }
    }
}
