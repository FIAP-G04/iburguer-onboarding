using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using iBurguer.Onboarding.Application.SignUp;
using iBurguer.Onboarding.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace iBurguer.Onboarding.SignUp;

public class Function
{
    readonly ServiceProvider _serviceProvider;
    
    public Function()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddiBurguerAuth();
        
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    
    public async Task<APIGatewayProxyResponse> FunctionHandler(SignUpRequest request, ILambdaContext context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var useCase = scope.ServiceProvider.GetRequiredService<ISignUpUseCase>();

            try
            {
                await useCase.SignUp(request);
                
                return new APIGatewayProxyResponse 
                {
                    StatusCode = (int)HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }

    private APIGatewayProxyResponse HandleException(Exception ex)
    {
        var statusCode = ex is DomainException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;

        return new APIGatewayProxyResponse 
        {
            StatusCode = (int)statusCode,
            Body = ex.Message
        };
    }
}

