namespace iBurguer.Onboarding.Domain;

public sealed record PersonName
{
    private PersonName(string firstName, string lastName)
    {
        firstName = firstName.TrimStart().TrimEnd();
        lastName = lastName.TrimStart().TrimEnd();
        
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            throw new DomainException(Errors.NameRequired);

        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public static PersonName From(string firstName, string lastName) => new(firstName, lastName);

    public override string ToString() => $"{FirstName} {LastName}";

    public static bool operator ==(PersonName personName, string name) =>
        string.Compare(personName.ToString(), name, StringComparison.InvariantCultureIgnoreCase) is 0;

    public static bool operator !=(PersonName personName, string name) =>
        string.Compare(personName.ToString(), name,
            StringComparison.InvariantCultureIgnoreCase) is not 0;

    public static class Errors
    {
        public static readonly string NameRequired =
            "É obrigatório informar nome e sobrenome do cliente";
    }
}