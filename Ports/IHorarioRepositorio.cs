using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IHorarioRepositorio
    {
        Task GuardarHorariosGenerados(int idUsuario, List<Horario> horariosGenerados);
        Task<List<List<Horario>>> ObtenerHorariosGuardados(int idUsuario);
    }
}
