using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for UpdateCustomerHandler tests.
/// </summary>
public static class UpdateCustomerHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateCustomerCommand instances.
    /// </summary>
    private static readonly Faker<UpdateCustomerCommand> UpdateCustomerCommandFaker = new Faker<UpdateCustomerCommand>()
        .RuleFor(c => c.Id, f => MongoDB.Bson.ObjectId.GenerateNewId().ToString())
        .RuleFor(c => c.Name, f => f.Name.FullName())
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"))
        .RuleFor(c => c.Address, f => f.Address.FullAddress());

    /// <summary>
    /// Generates a valid UpdateCustomerCommand with random data.
    /// </summary>
    /// <returns>A valid UpdateCustomerCommand</returns>
    public static UpdateCustomerCommand GenerateValidCommand()
    {
        return UpdateCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an UpdateCustomerCommand with an invalid email.
    /// </summary>
    /// <returns>An UpdateCustomerCommand with an invalid email</returns>
    public static UpdateCustomerCommand GenerateCommandWithInvalidEmail()
    {
        var command = GenerateValidCommand();
        command.Email = "invalid-email";
        return command;
    }

    /// <summary>
    /// Generates an UpdateCustomerCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>An UpdateCustomerCommand with the specified ID</returns>
    public static UpdateCustomerCommand GenerateCommandWithId(string id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}
