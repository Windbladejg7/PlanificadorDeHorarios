namespace PlanificadorDeHorarios.Api.Domain
{
    public class Horario
    {
        public Dictionary<string, Aula> selecciones { get; set; }

        public Horario(Dictionary<string, Aula> dictionary)
        {
            selecciones = dictionary
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void Agregar(string materia, Aula aula)
        {
            selecciones.Add(materia, aula);
        }
    }
}
