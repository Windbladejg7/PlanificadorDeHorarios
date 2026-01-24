using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Common
{
    public interface ITokenGenerator
    {
        string GenerarToken(Usuario usuario);
    }
}
