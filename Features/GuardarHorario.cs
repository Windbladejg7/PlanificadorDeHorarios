using PlanificadorDeHorarios.Api.Ports;
using PlanificadorDeHorarios.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using PlanificadorDeHorarios.Api.Common;
using System.Security.Claims;

namespace PlanificadorDeHorarios.Api.Features
{
    public record GuardarHorarioRequest(Horario horario);
    public class GuardarHorarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) 
        {
            app.MapPost("horarios/guardar", async (ClaimsPrincipal usuario, GuardarHorarioRequest request, [FromServices] GuardarHorarioHandler handler) =>
            {
                int idUsuario = int.Parse(usuario.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                Console.WriteLine(idUsuario);
                var result = await handler.Handle(idUsuario, request);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).RequireAuthorization();
        }
    }

    public class GuardarHorarioHandler : IHandler 
    {
        IHorarioRepositorio _repositorio;

        public GuardarHorarioHandler(IHorarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<string>> Handle(int idUsuario, GuardarHorarioRequest request)
        {
            try
            {
                await _repositorio.GuardarHorario(idUsuario, request.horario);
                return Result<string>.Success("Horario guardado exitosamente");
            }catch(Exception ex)
            {   
                return Result<string>.Failure("Error al guardar horario "+ex.Message);
            }
        }
    }
}
