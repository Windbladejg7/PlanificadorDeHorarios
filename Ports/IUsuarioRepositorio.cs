using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IUsuarioRepositorio
    {
        Task AgregarUsuarioAsync(Usuario usuario);
        Task<bool> VerificarSiExistePorEmailAsync(string email);
        Task<Usuario> BuscarPorEmailAsync(string email);
        Task<List<Usuario>> ObtenerTodosAsync();
    }
}
