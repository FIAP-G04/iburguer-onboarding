namespace iBurguer.Onboarding.Application.Disable
{
    public interface IDisableUserUseCase
    {
        Task<DisableUserResponse> Disable(string cpf);
    }
    public class DisableUserUseCase : IDisableUserUseCase
    {
        private readonly IIdentityGateway _identityGateway;

        public DisableUserUseCase(IIdentityGateway identityGateway)
        {
            ArgumentNullException.ThrowIfNull(identityGateway);

            _identityGateway = identityGateway;
        }

        public async Task<DisableUserResponse> Disable(string cpf)
        {
            var result = await _identityGateway.DisableUserAsync(cpf);

            return new(result);
        }
    }
}
