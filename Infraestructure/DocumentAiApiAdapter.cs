using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class DocumentAiApiAdapter : IOcrApiAdapter
    {
        public async Task<List<Materia>> OcrAsync(IFormFile file)
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
            return ResponseMapper.Mapear(response);
        }
    }
}
