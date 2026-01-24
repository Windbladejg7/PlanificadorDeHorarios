using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Ports
{
    public interface IUsuarioRepositorio
    {
        void AgregarUsuario(Usuario usuario);
        bool VerificarSiExistePorEmail(string email);
        Usuario BuscarPorEmail(string email);
        List<Usuario> ObtenerTodos();
    }
}
