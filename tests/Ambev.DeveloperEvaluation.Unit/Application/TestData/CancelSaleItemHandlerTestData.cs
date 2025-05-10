using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CancelSaleItemHandler tests.
/// </summary>
public static class CancelSaleItemHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CancelSaleItemCommand instances.
    /// </summary>
    private static readonly Faker<CancelSaleItemCommand> CancelSaleItemCommandFaker = new Faker<CancelSaleItemCommand>()
        .RuleFor(c => c.SaleId, f => Guid.NewGuid())
        .RuleFor(c => c.ProductId, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid CancelSaleItemCommand with random data.
    /// </summary>
    /// <returns>A valid CancelSaleItemCommand</returns>
    public static CancelSaleItemCommand GenerateValidCommand()
    {
        return CancelSaleItemCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CancelSaleItemCommand with specific IDs.
    /// </summary>
    /// <param name="saleId">The sale ID to use</param>
    /// <param name="productId">The product ID to use</param>
    /// <returns>A CancelSaleItemCommand with the specified IDs</returns>
    public static CancelSaleItemCommand GenerateCommandWithIds(Guid saleId, Guid productId)
    {
        var command = GenerateValidCommand();
        command.SaleId = saleId;
        command.ProductId = productId;
        return command;
    }
}