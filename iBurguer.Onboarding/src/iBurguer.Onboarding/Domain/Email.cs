using System.Text.RegularExpressions;

namespace iBurguer.Onboarding.Domain;


public sealed record Email
{
    private const string emailPattern =
        @"^[\w-\.]+@[A-Za-z0-9]([A-Za-z0-9-]+\.)+[A-Za-z0-9-]{2,4}$";
    
    public Email(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new DomainException(Errors.EmailRequired);

        if (!new Regex(emailPattern).IsMatch(email))
            throw new DomainException(Errors.InvalidEmail);

        Value = email;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;

    public static implicit operator Email(string email) => new(email);

    public static class Errors
    {
        public static readonly string EmailRequired = "O email não pode estar vazio ou em branco.";

        public static readonly string InvalidEmail =
            "A cadeia de caracteres informada não está em um formato válido.";
    }
}