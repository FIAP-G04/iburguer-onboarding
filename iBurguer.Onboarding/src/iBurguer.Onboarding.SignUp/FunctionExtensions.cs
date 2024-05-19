using Amazon.CognitoIdentityProvider;
using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.SignUp;
using iBurguer.Onboarding.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.Onboarding.SignUp;

[ExcludeFromCodeCoverage]
public static class FunctionExtensions
{
    public static IServiceCollection AddiBurguerAuth(this ServiceCollection services)
    {
        services.AddTransient<ISignUpUseCase, SignUpUseCase>()
                .AddTransient<IIdentityGateway, CognitoGateway>()
                .AddAWSService<IAmazonCognitoIdentityProvider>();
        
        return services;
    }
}