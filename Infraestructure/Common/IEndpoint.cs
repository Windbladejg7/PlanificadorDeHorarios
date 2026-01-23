namespace PlanificadorDeHorarios.Api.Infraestructure.Common
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouterBuilder app);
    }
}