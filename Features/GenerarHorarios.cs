using Microsoft.AspNetCore.Mvc;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Features
{
    public record GenerarHorariosRequest();
    public class GenerarHorariosEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/horarios", async (GenerarHorariosRequest request, [FromServices] GenerarHorarioHandler handler) =>
            {
                await handler.Handle(request);
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

        public async Task Handle(GenerarHorariosRequest request)
        {
            
        }
    }
}
