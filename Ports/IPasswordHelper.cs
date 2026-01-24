using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IPasswordHelper
    {
        string HashearPassword(string password);
        bool VerificarPassword(string password, string hashedPassword);
    }
}
