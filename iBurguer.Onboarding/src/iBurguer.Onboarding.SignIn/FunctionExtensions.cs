using Amazon.CognitoIdentityProvider;
using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.SignIn;
using iBurguer.Onboarding.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.Onboarding.SignIn;

[ExcludeFromCodeCoverage]
public static class FunctionExtensions
{
    public static IServiceCollection AddiBurguerAuth(this ServiceCollection services)
    {
        services.AddTransient<ISignInUseCase, SignInUseCase>()
                .AddTransient<IIdentityGateway, CognitoGateway>()
                .AddAWSService<IAmazonCognitoIdentityProvider>();
        
        return services;
    }
}