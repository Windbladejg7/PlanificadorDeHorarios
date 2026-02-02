using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IOcrApiAdapter
    {
        public Task<List<Materia>> OcrAsync(IFormFile file);
    }
}