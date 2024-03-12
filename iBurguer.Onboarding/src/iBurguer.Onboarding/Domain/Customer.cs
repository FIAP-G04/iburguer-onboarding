namespace iBurguer.Onboarding.Domain;


public class Customer 
{
    public Id Id{ get; private set; }
    
    public CPF CPF { get; private set; }

    public PersonName Name { get; private set; }

    public Email Email { get; private set; }

    public Customer(string cpf, PersonName name, Email email)
    {
        Id = Id.New;
        CPF = cpf;
        Name = name;
        Email = email;
    }
}