namespace PlanificadorDeHorarios.Api.Domain
{
    public class Aula
    {
        public string Nombre { get; set; }
        public List<BloqueHorario> Bloques { get; set; } = [];
    }
}
