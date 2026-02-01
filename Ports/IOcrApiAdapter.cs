using Google.Cloud.DocumentAI.V1;
using PlanificadorDeHorarios.Api.Common;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IOcrApiAdapter
    {
        public Task<Document> OcrAsync(IFormFile file);
    }
}