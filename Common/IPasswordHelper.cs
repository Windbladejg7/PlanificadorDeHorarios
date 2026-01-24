using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Common
{
    public interface IPasswordHelper
    {
        string HashearPassword(string password);
        bool VerificarPassword(string password, string hashedPassword);
    }
}
