using Ambev.DeveloperEvaluation.Application.Products.UpdateStock;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for UpdateStockHandler tests.
/// </summary>
public static class UpdateStockHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateStockCommand instances.
    /// </summary>
    private static readonly Faker<UpdateStockCommand> UpdateStockCommandFaker = new Faker<UpdateStockCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.Quantity, f => f.Random.Int(1, 100));

    /// <summary>
    /// Generates a valid UpdateStockCommand with random data.
    /// </summary>
    /// <returns>A valid UpdateStockCommand</returns>
    public static UpdateStockCommand GenerateValidCommand()
    {
        return UpdateStockCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an UpdateStockCommand with negative quantity.
    /// </summary>
    /// <returns>An UpdateStockCommand with negative quantity</returns>
    public static UpdateStockCommand GenerateCommandWithNegativeQuantity()
    {
        var command = GenerateValidCommand();
        command.Quantity = -10;
        return command;
    }
}