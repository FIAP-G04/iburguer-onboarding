using FluentAssertions;
using iBurguer.Onboarding.Domain;

namespace iBurguer.Onboarding.Tests;

public class PersonNameTest
{
    [Theory]
    [InlineData("Anna", "Lembke", "Anna Lembke")]
    [InlineData("John Migliori", "Kort", "John Migliori Kort")]
    [InlineData("Erich", "Martin Johnson", "Erich Martin Johnson")]
    public void ShouldValidPersonName(string firstName, string lastName, string fullName) =>
        //Act & Assert
        PersonName.From(firstName, lastName).ToString().Should().Be(fullName);
    
    [Theory]
    [InlineData("Anna", "")]
    [InlineData("", "Lembke")]
    [InlineData("", "")]
    [InlineData("Anna", "     ")]
    [InlineData("     ", "Lembke")]
    [InlineData("     ", "    ")]
    public void ShouldThrowErrorWhenFirstNameOrLastNameNotProvided(string firstName, string lastName)
    {
        //Act
        var action = () => PersonName.From(firstName, lastName);

        //Assert
        action.Should().Throw<DomainException>()
            .WithMessage(PersonName.Errors.NameRequired);
    }
}