using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace PlanificadorDeHorarios.Api.Features
{
    public record CargarImagenRequest (IFormFile file);
    public class CargarImagenEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("imagen/subir", async ([FromForm]CargarImagenRequest request, [FromServices] CargarImagenHandler handler) =>
            {
                var result = await handler.Handle(request);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.InternalServerError();
            }).DisableAntiforgery();
        }
    }

    public class CargarImagenHandler : IHandler
    {
        private IOcrApiAdapter _ocrApi;

        public CargarImagenHandler(IOcrApiAdapter ocrApi)
        {
            this._ocrApi = ocrApi;
        }

        public async Task<Result<List<Materia>>> Handle(CargarImagenRequest request)
        {
            var respuesta = await _ocrApi.OcrAsync(request.file);

            if (respuesta == null)
                return Result<List<Materia>>.Failure("Error al extraer los datos");

            return Result<List<Materia>>.Success(respuesta);
        }
    }
}
