using Google.Cloud.DocumentAI.V1;
using PlanificadorDeHorarios.Api.Domain;
using System.Text.RegularExpressions;

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
                string? aulaNombre = e.Properties
                    .Where(p => p.Type == "group_description")
                    .Select(p => p.MentionText)?
                    .FirstOrDefault();
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
				                string nombreNormalizado = Regex.Replace(m.MentionText, @"[- \n\r\t]+", " ").Trim();
                                if (!materias.TryGetValue(nombreNormalizado, out materia))
                                {
                                    materia = new Materia();
                                    materia.OpcionesDeAula = [];
                                    materia.Nombre = m.MentionText;
                                    materias.Add(nombreNormalizado, materia);
                                }
                            }

                            if (m.Type.Contains("schedule"))
                            {
                                var matches = Regex.Matches(m.MentionText, @"\d{1,2}:\d{2}");

				if (matches.Count >= 2) 
				{
    					aula.Bloques.Add(new()
    					{
        					Dia = m.Type.Split('_')[0],
        					HoraInicio = TimeSpan.Parse(matches[0].Value),
        					HoraFin = TimeSpan.Parse(matches[matches.Count - 1].Value)
    					});
				}
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
