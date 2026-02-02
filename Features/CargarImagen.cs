using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.DocumentAI.V1;

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
                return result.IsSuccess ? Results.Ok() : Results.InternalServerError();
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

        public async Task<Result<Document>> Handle(CargarImagenRequest request)
        {
            var respuesta = await _ocrApi.OcrAsync(request.file);

            if (respuesta == null)
                return Result<Document>.Failure("Error al extraer los datos");

            return Result<Document>.Success(respuesta);
        }
    }
}
