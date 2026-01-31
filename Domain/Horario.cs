namespace PlanificadorDeHorarios.Api.Domain
{
    public class Horario
    {
        private Dictionary<Materia, Aula> horario;
        
        public Horario(Dictionary<Materia, Aula> horario)
        {
            this.horario = horario.ToDictionary(
                pair => pair.Key, 
                pair => pair.Value
                );
        }

        public void Agregar(Materia materia, Aula aula)
        {
            horario.Add(materia, aula);
        }
    }
}
