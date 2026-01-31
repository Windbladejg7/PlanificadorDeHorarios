using System.Reflection.Metadata.Ecma335;

namespace PlanificadorDeHorarios.Api.Domain
{
    public class Materia
    {
        public string Nombre { get; set; }
        public List<Aula> OpcionesDeAula { get; set; }
    }
}
