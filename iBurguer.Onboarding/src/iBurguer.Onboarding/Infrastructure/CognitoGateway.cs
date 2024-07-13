using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Security.Cryptography;
using System.Text;
using iBurguer.Onboarding.Application;
using iBurguer.Onboarding.Application.SignIn;
using iBurguer.Onboarding.Domain;

namespace iBurguer.Onboarding.Infrastructure;

public class CognitoGateway : IIdentityGateway
{
    private readonly string ClientId;
    private readonly string UserPoolId;
    
    private readonly IAmazonCognitoIdentityProvider _cognitoService;
    
    public CognitoGateway(IAmazonCognitoIdentityProvider cognitoService)
    {
        ArgumentNullException.ThrowIfNull(cognitoService);
        
        _cognitoService = cognitoService;

        ClientId = Environment.GetEnvironmentVariable("CLIENT_ID")!;
        UserPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID")!;
    }
    
    public async Task<bool> SignUpAsync(Customer customer)
    {
        var attributes = new List<AttributeType>();

        attributes.Add(new AttributeType{ Name = "email", Value = customer.Email });
        attributes.Add(new AttributeType{ Name = "custom:customerId", Value = customer.Id.ToString() });
        attributes.Add(new AttributeType{ Name = "given_name", Value = customer.Name.FirstName });
        attributes.Add(new AttributeType{ Name = "family_name", Value = customer.Name.LastName });
        
        var signUpRequest = new SignUpRequest
        {
            UserAttributes = attributes,
            Username = customer.CPF,
            ClientId = ClientId,
            Password = GeneratePassword(customer.CPF),
        };
        
        var response = await _cognitoService.SignUpAsync(signUpRequest);

        if (response is not null)
        {
            await ConfirmedSignUp(customer);
            
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        return false;
    }

    private async Task<bool> ConfirmedSignUp(Customer customer)
    {
        var confirmRequest = new AdminConfirmSignUpRequest
        {
            Username = customer.CPF,
            UserPoolId = UserPoolId
        };

        var response = await _cognitoService.AdminConfirmSignUpAsync(confirmRequest);
        
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<SignInResponse?> SignInAsync(CPF cpf)
    {
        Console.WriteLine($"Client: {ClientId} - Pool: {UserPoolId}");
        var parameters = new Dictionary<string, string>();
        
        parameters.Add("USERNAME", cpf);
        parameters.Add("PASSWORD", GeneratePassword(cpf));

        var authRequest = new InitiateAuthRequest
        {
            ClientId = ClientId,
            AuthParameters = parameters,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
        };

        try
        {
            var response = await _cognitoService.InitiateAuthAsync(authRequest);

            return new SignInResponse(
                response.AuthenticationResult.AccessToken,
                response.AuthenticationResult.TokenType,
                response.AuthenticationResult.ExpiresIn,
                response.AuthenticationResult.RefreshToken,
                response.AuthenticationResult.IdToken
            );
        }
        catch (NotAuthorizedException ex)
        {
            return null;
        }
    }

    private string GeneratePassword(string cpf)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(cpf);

        SHA256 sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(dataBytes);
        
        return Convert.ToBase64String(hash);
    }

    public async Task<bool> DisableUserAsync(CPF cpf)
    {
        var request = new AdminDeleteUserRequest()
        {
            Username = cpf,
            UserPoolId = UserPoolId
        };

        var response = await _cognitoService.AdminDeleteUserAsync(request);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}