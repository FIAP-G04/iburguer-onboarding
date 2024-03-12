namespace iBurguer.Onboarding.Domain;

public class DomainException : Exception
{
    public DomainException(string message, params object?[] values) : base(
        string.Format(message, values))
    {
    }
}