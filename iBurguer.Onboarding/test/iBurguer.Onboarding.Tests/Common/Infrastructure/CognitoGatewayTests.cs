using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using iBurguer.Onboarding.Domain;
using iBurguer.Onboarding.Infrastructure;
using Moq;

namespace iBurguer.Onboarding.UnitTests.Infrastructure
{
    public class CognitoGatewayTests
    {
        private readonly Mock<IAmazonCognitoIdentityProvider> _cognitoServiceMock;
        private readonly CognitoGateway _cognitoGateway;

        public CognitoGatewayTests()
        {
            _cognitoServiceMock = new Mock<IAmazonCognitoIdentityProvider>();
            _cognitoGateway = new CognitoGateway(_cognitoServiceMock.Object);
        }

        [Fact]
        public async Task SignUpAsync_ReturnsTrue_When_SignUpSucceeds()
        {
            // Arrange
            var customer = new Customer("14755521068", PersonName.From("John", "Doe"), new Email("test@example.com"));

            _cognitoServiceMock.Setup(s => s.SignUpAsync(It.IsAny<SignUpRequest>(), default))
                .ReturnsAsync(new SignUpResponse { HttpStatusCode = HttpStatusCode.OK });

            _cognitoServiceMock.Setup(s => s.AdminConfirmSignUpAsync(It.IsAny<AdminConfirmSignUpRequest>(), default))
                .ReturnsAsync(new AdminConfirmSignUpResponse { HttpStatusCode = HttpStatusCode.OK });

            // Act
            var result = await _cognitoGateway.SignUpAsync(customer);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SignUpAsync_ReturnsFalse_When_SignUpFails()
        {
            // Arrange
            var customer = new Customer("14755521068", PersonName.From("John", "Doe"), new Email("test@example.com"));

            _cognitoServiceMock.Setup(s => s.SignUpAsync(It.IsAny<SignUpRequest>(), default))
                .ReturnsAsync((SignUpResponse)null);

            // Act
            var result = await _cognitoGateway.SignUpAsync(customer);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SignInAsync_Returns_SignInResponse_When_SignInSucceeds()
        {
            // Arrange
            var cpf = "14755521068";
            var accessToken = "access_token";
            var tokenType = "token_type";
            var expiresIn = 3600;
            var refreshToken = "refresh_token";
            var idToken = "id_token";

            var response = new InitiateAuthResponse
            {
                HttpStatusCode = HttpStatusCode.OK,
                AuthenticationResult = new AuthenticationResultType
                {
                    AccessToken = accessToken,
                    TokenType = tokenType,
                    ExpiresIn = expiresIn,
                    RefreshToken = refreshToken,
                    IdToken = idToken
                }
            };

            _cognitoServiceMock.Setup(s => s.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _cognitoGateway.SignInAsync(cpf);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accessToken, result.AccessToken);
            Assert.Equal(tokenType, result.TokenType);
            Assert.Equal(expiresIn, result.ExpiresIn);
            Assert.Equal(refreshToken, result.RefreshToken);
            Assert.Equal(idToken, result.TokenId);
        }

        [Fact]
        public async Task SignInAsync_ReturnsNull_When_NotAuthorizedExceptionIsThrown()
        {
            // Arrange
            var cpf = "14755521068";

            _cognitoServiceMock.Setup(s => s.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), default))
                .ThrowsAsync(new NotAuthorizedException(""));

            // Act
            var result = await _cognitoGateway.SignInAsync(cpf);

            // Assert
            Assert.Null(result);
        }
    }
}
