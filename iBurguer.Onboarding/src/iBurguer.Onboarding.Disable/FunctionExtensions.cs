using Amazon.CognitoIdentityProvider;
using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.Disable;
using iBurguer.Onboarding.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.Onboarding.Disable;

[ExcludeFromCodeCoverage]
public static class FunctionExtensions
{
    public static IServiceCollection AddiBurguerAuth(this ServiceCollection services)
    {
        services.AddTransient<IDisableUserUseCase, DisableUserUseCase>()
                .AddTransient<IIdentityGateway, CognitoGateway>()
                .AddAWSService<IAmazonCognitoIdentityProvider>();
        
        return services;
    }
}