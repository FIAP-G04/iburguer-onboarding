using iBurguer.Onboarding.Domain;
using iBurguer.Onboarding.Infrastructure;

namespace iBurguer.Onboarding.Application.SignUp;

public interface ISignUpUseCase
{
    Task<bool> SignUp(SignUpRequest request);
}

public class SignUpUseCase : ISignUpUseCase
{
    private readonly IIdentityGateway _gateway;

    public SignUpUseCase(IIdentityGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(gateway);
        
        _gateway = gateway;
    }

    public async Task<bool> SignUp(SignUpRequest request)
    {
        var customer = new Customer(request.Cpf, PersonName.From(request.FirstName, request.LastName), request.Email);
        
        return await _gateway.SignUpAsync(customer);
    }
}