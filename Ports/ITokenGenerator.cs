using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface ITokenGenerator
    {
        string GenerarToken(Usuario usuario);
    }
}
