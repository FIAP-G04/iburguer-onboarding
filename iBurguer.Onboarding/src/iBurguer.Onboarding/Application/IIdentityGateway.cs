using iBurguer.Onboarding.Application.SignIn;
using iBurguer.Onboarding.Domain;

namespace iBurguer.Onboarding.Application;

public interface IIdentityGateway
{
    Task<bool> SignUpAsync(Customer customer);

    Task<SignInResponse?> SignInAsync(CPF cpf);
}