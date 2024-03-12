using iBurguer.Onboarding.Domain;
using iBurguer.Onboarding.Infrastructure;

namespace iBurguer.Onboarding.Application.SignIn;

public interface ISignInUseCase
{
    Task<SignInResponse> SignIn(string cpf);
}

public class SignInUseCase : ISignInUseCase
{
    private readonly IIdentityGateway _gateway;

    public SignInUseCase(IIdentityGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(gateway);

        _gateway = gateway;
    }

    public async Task<SignInResponse> SignIn(string cpf)
    {
        var response = await _gateway.SignInAsync(cpf);

        if (response is null)
        {
            throw new UnidentifiedCustomerException(cpf);
        }

        return response;
    }
}