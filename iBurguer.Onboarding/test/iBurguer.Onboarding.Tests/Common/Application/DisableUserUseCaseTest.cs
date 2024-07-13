using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.Disable;
using iBurguer.Onboarding.Domain;
using Moq;

namespace iBurguer.Onboarding.Tests.Common.Application
{
    public class DisableUserUseCaseTest
    {
        private readonly DisableUserUseCase _usecase;

        private readonly Mock<IIdentityGateway> _gateway = new();

        public DisableUserUseCaseTest()
        {
            _usecase = new(_gateway.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldDisableUser(bool gatewayResult)
        {
            var cpf = "14755521068";

            _gateway.Setup(s => s.DisableUserAsync(It.IsAny<CPF>()))
                .ReturnsAsync(gatewayResult);

            var result = await _usecase.Disable(cpf);

            Assert.Equal(gatewayResult, result.Success);
        }
    }
}
