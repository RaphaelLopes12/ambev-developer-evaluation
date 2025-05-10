using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CancelSaleHandler tests.
/// </summary>
public static class CancelSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CancelSaleCommand instances.
    /// </summary>
    private static readonly Faker<CancelSaleCommand> CancelSaleCommandFaker = new Faker<CancelSaleCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid CancelSaleCommand with random data.
    /// </summary>
    /// <returns>A valid CancelSaleCommand</returns>
    public static CancelSaleCommand GenerateValidCommand()
    {
        return CancelSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CancelSaleCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>A CancelSaleCommand with the specified ID</returns>
    public static CancelSaleCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}