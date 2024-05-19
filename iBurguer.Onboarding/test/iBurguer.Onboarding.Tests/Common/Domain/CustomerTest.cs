using FluentAssertions;
using iBurguer.Onboarding.Domain;

namespace iBurguer.Onboarding.Tests.Common.Domain;

public class CustomerTest
{
    [Fact]
    public void ShouldCreateCustomer()
    {
        //Arrange
        var cpf = "12345678909";
        var name = PersonName.From("Customer", "Name");
        var email = new Email("customer.email@fiap.com");

        //Act
        var customer = new Customer(cpf, name, email);

        //Assert
        customer.Id.Should().NotBeNull();
        customer.Id.Value.Should().NotBe(Guid.Empty);
        customer.CPF.Number.Should().Be(cpf);
        customer.Name.Should().Be(name);
        customer.Email.Value.Should().Be(email);
    }
}