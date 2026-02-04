using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IHorarioRepositorio
    {
        Task GuardarHorario(int idUsuario, Horario horario);
        Task<List<Horario>> ObtenerHorariosGuardados(int idUsuario);
    }
}
