using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for DeleteCustomerHandler tests.
/// </summary>
public static class DeleteCustomerHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid DeleteCustomerCommand instances.
    /// </summary>
    private static readonly Faker<DeleteCustomerCommand> DeleteCustomerCommandFaker = new Faker<DeleteCustomerCommand>()
        .RuleFor(c => c.Id, f => MongoDB.Bson.ObjectId.GenerateNewId().ToString());

    /// <summary>
    /// Generates a valid DeleteCustomerCommand with random data.
    /// </summary>
    /// <returns>A valid DeleteCustomerCommand</returns>
    public static DeleteCustomerCommand GenerateValidCommand()
    {
        return DeleteCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a DeleteCustomerCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>A DeleteCustomerCommand with the specified ID</returns>
    public static DeleteCustomerCommand GenerateCommandWithId(string id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}
