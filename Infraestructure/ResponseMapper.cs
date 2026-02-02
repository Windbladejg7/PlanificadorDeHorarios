using Google.Cloud.DocumentAI.V1;
using PlanificadorDeHorarios.Api.Domain;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class ResponseMapper
    {
        public static List<Materia> Mapear(ProcessResponse response) 
        {
            Dictionary<string, Materia> materias = [];

            foreach (var e in response.Document.Entities)
            {
                Materia? materia = null;
                string aulaNombre = e.Properties
                    .Where(p => p.Type == "group_description")
                    .Select(p => p.MentionText)
                    .First();
                foreach (var p in e.Properties)
                {
                    if (p.Type == "course")
                    {
                        Aula aula = new Aula();
                        aula.Nombre = aulaNombre;
                        foreach (var m in p.Properties)
                        {
                            if (m.Type == "course_name")
                            {
                                if (!materias.TryGetValue(m.MentionText, out materia))
                                {
                                    materia = new Materia();
                                    materia.OpcionesDeAula = [];
                                    materia.Nombre = m.MentionText;
                                    materias.Add(materia.Nombre, materia);
                                }
                            }

                            if (m.Type.Contains("schedule"))
                            {
                                var dia = m.Type.Split('_')[0];
                                var horas = m.MentionText.Split('-');
                                aula.Bloques.Add(new()
                                {
                                    Dia = dia,
                                    HoraInicio = TimeSpan.Parse(horas[0]),
                                    HoraFin = TimeSpan.Parse(horas[1])
                                });
                            }
                        }
                        materia.OpcionesDeAula.Add(aula);
                    }
                }

            }
            return materias.Values.ToList();
        }
    }
}
