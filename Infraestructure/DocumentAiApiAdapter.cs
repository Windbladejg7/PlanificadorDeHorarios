using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using PlanificadorDeHorarios.Api.Common;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class DocumentAiApiAdapter : IOcrApiAdapter
    {
        private readonly GoogleCloudOptions _options;

        public DocumentAiApiAdapter(IOptions<GoogleCloudOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<Materia>> OcrAsync(IFormFile file)
        {
            DocumentProcessorServiceClient cliente = await DocumentProcessorServiceClient.CreateAsync();

            ProcessorName processorName = new ProcessorName(
                _options.ProjectId,
                _options.Location,
                _options.ProcessorId
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
