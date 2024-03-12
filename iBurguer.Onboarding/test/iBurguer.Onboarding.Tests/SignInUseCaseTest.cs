using AutoFixture;
using FluentAssertions;
using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.SignIn;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace iBurguer.Onboarding.Tests;

public class SignInUseCaseTest
{
    private readonly IIdentityGateway _gateway;
    private readonly ISignInUseCase _sut;
    private readonly Fixture _fixture;
    private readonly string _cpf;

    public SignInUseCaseTest()
    {
        _gateway = Substitute.For<IIdentityGateway>();
        _sut = new SignInUseCase(_gateway);
        _fixture = new Fixture();
        _cpf = "46793281771";
    }
    
    [Fact]
    public void ShouldThrowErrorWhenIdentityGatewayNotProvided()
    {
        //Act
        var action = () => new SignInUseCase(null);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task ShouldSignInCustomer()
    {
        //Arrange
        var response = _fixture.Create<SignInResponse>();
        _gateway.SignInAsync(_cpf).Returns(response); 

        //Act
        var result = await _sut.SignIn(_cpf);

        //Assert
        result.AccessToken.Should().Be(response.AccessToken);
        result.RefreshToken.Should().Be(response.RefreshToken);
        result.TokenType.Should().Be(response.TokenType);
        result.TokenId.Should().Be(response.TokenId);
        result.ExpiresIn.Should().Be(response.ExpiresIn);
    }

    [Fact]
    public async Task ShouldThrowErrorWhenCustomerUnidentified()
    {
        //Arrange
        _gateway.SignInAsync(_cpf).ReturnsNullForAnyArgs(); 

        //Act
        var action = async () => await _sut.SignIn(_cpf);

        //Assert
        await action.Should().ThrowAsync<UnidentifiedCustomerException>()
            .WithMessage(string.Format(UnidentifiedCustomerException.error, _cpf));
    }
}
