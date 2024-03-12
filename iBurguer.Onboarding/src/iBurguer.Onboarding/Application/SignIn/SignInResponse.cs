namespace iBurguer.Onboarding.Application.SignIn;

public record SignInResponse(string AccessToken, string TokenType, int ExpiresIn, string RefreshToken, string TokenId);