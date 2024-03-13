using System.Text.Json.Serialization;

namespace iBurguer.Onboarding.Application.SignUp;

public record SignUpRequestGateway(string Body);

public class SignUpRequest
{
    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
}