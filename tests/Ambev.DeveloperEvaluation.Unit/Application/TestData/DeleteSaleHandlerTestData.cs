using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for DeleteSaleHandler tests.
/// </summary>
public static class DeleteSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid DeleteSaleCommand instances.
    /// </summary>
    private static readonly Faker<DeleteSaleCommand> DeleteSaleCommandFaker = new Faker<DeleteSaleCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid DeleteSaleCommand with random data.
    /// </summary>
    /// <returns>A valid DeleteSaleCommand</returns>
    public static DeleteSaleCommand GenerateValidCommand()
    {
        return DeleteSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a DeleteSaleCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>A DeleteSaleCommand with the specified ID</returns>
    public static DeleteSaleCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}