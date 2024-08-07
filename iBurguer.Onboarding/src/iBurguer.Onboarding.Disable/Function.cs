using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using iBurguer.Onboarding.Application.Disable;
using iBurguer.Onboarding.Domain;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace iBurguer.Onboarding.Disable;

[ExcludeFromCodeCoverage]
public class Function
{
    private readonly ServiceProvider _serviceProvider;

    public Function()
    {

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddiBurguerAuth();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(DisableUserRequestGateway request, ILambdaContext context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var useCase = scope.ServiceProvider.GetRequiredService<IDisableUserUseCase>();

            try
            {
                var response = await useCase.Disable(request.Body);

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonSerializer.Serialize(response)
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
        var statusCode = ex switch
        {
            DomainException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = ex.Message
        };
    }
}

