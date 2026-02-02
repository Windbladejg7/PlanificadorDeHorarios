using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;
using System.Numerics;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class DocumentAiApiAdapter : IOcrApiAdapter
    {
        public async Task<Document> OcrAsync(IFormFile file)
        {
            string processorId = "d938229754c2ed4e";
            DocumentProcessorServiceClient cliente = await DocumentProcessorServiceClient.CreateAsync();

            ProcessorName processorName = new ProcessorName(
                "apidocumentai",
                "us",
                processorId
                );

            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);
            byte[] fileBytes = ms.ToArray();

            ProcessRequest request = new ProcessRequest
            {
                Name = processorName.ToString(),
                RawDocument = new RawDocument
                {
                    Content = ByteString.CopyFrom(fileBytes),
                    MimeType = file.ContentType
                }
            };

            ProcessResponse response = await cliente.ProcessDocumentAsync(request);

            Dictionary<string, Materia> materias = [];

            foreach (var e in response.Document.Entities)
            {
                string aulaNombre = null;
                foreach (var p in e.Properties)
                {
                    if (p.Type == "group_description")
                    {
                        aulaNombre = p.MentionText;
                        Console.WriteLine(aulaNombre);
                    }

                    if (p.Type == "course")
                    {
                        Materia? materia = null;
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

            foreach (var k in materias.Keys)
            {
                Console.WriteLine(k);
                foreach(var v in materias[k].OpcionesDeAula)
                {
                    Console.WriteLine(v.Nombre);
                }
            }

            return response.Document;
        }
    }
}
