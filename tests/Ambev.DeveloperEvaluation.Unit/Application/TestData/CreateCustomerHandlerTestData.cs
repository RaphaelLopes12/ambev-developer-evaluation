using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateCustomerHandler tests.
/// </summary>
public static class CreateCustomerHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateCustomerCommand instances.
    /// </summary>
    private static readonly Faker<CreateCustomerCommand> CreateCustomerCommandFaker = new Faker<CreateCustomerCommand>()
        .RuleFor(c => c.Name, f => f.Name.FullName())
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"))
        .RuleFor(c => c.Address, f => f.Address.FullAddress());

    /// <summary>
    /// Generates a valid CreateCustomerCommand with random data.
    /// </summary>
    /// <returns>A valid CreateCustomerCommand</returns>
    public static CreateCustomerCommand GenerateValidCommand()
    {
        return CreateCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateCustomerCommand with an invalid email.
    /// </summary>
    /// <returns>A CreateCustomerCommand with an invalid email</returns>
    public static CreateCustomerCommand GenerateCommandWithInvalidEmail()
    {
        var command = GenerateValidCommand();
        command.Email = "invalid-email";
        return command;
    }

    /// <summary>
    /// Generates a CreateCustomerCommand with an empty name.
    /// </summary>
    /// <returns>A CreateCustomerCommand with an empty name</returns>
    public static CreateCustomerCommand GenerateCommandWithEmptyName()
    {
        var command = GenerateValidCommand();
        command.Name = string.Empty;
        return command;
    }
}
