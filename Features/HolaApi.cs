using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Features
{
    public class HolaApiEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/", () =>
            {
                return Results.Ok("Hola Mundo");
            });
        }
    }
}
