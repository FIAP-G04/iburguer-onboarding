using AutoFixture;
using FluentAssertions;
using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.SignUp;
using iBurguer.Onboarding.Domain;
using NSubstitute;

namespace iBurguer.Onboarding.Tests.Common.Application;

public class SignUpUseCaseTest
{
    private readonly IIdentityGateway _gateway;
    private readonly ISignUpUseCase _sut;
    private readonly Fixture _fixture;
    private SignUpRequest _signUpRequest;

    public SignUpUseCaseTest()
    {
        _gateway = Substitute.For<IIdentityGateway>();
        _sut = new SignUpUseCase(_gateway);
        _fixture = new Fixture();
        _signUpRequest = _fixture.Build<SignUpRequest>().With(r => r.Cpf, "46793281771")
            .With(r => r.Email, "anne@johnson.com").Create();
    }

    [Fact]
    public void ShouldThrowErrorWhenIdentityGatewayNotProvided()
    {
        //Act
        var action = () => new SignUpUseCase(null);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task ShouldSignUpCustomer()
    {
        //Arrange
        _gateway.SignUpAsync(Arg.Any<Customer>()).Returns(true);

        //Act
        var result = await _sut.SignUp(_signUpRequest);

        //Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("3859973681733")]
    [InlineData("11111111111")]
    [InlineData("45609899100")]
    public async Task ShouldThrowErrorWhenCpfNotProvidedOrInvalid(string cpf)
    {
        //Arrange
        _signUpRequest.Cpf = cpf;

        //Act
        var action = async () => await _sut.SignUp(_signUpRequest);

        //Assert
        await action.Should().ThrowAsync<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("anne@john")]
    [InlineData("@anne@gmail.com")]
    [InlineData("anne")]
    public async Task ShouldThrowErrorWhenEmailNotProvided(string email)
    {
        //Arrange
        _signUpRequest.Email = email;

        //Act
        var action = async () => await _sut.SignUp(_signUpRequest);

        //Assert
        await action.Should().ThrowAsync<DomainException>();
    }
}
