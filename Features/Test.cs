namespace PlanificadorDeHorarios.Api.Features
{
    public class TestRequest
    {
        
    }

    public class TestEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouterBuilder app)
        {
            app.MapGet("/", (TestRequest req)=>
            {
                
            });
        }
    }
}