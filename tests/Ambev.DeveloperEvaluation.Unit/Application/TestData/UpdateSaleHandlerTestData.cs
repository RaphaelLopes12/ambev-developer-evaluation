using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for UpdateSaleHandler tests.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateSaleCommand instances.
    /// </summary>
    private static readonly Faker<UpdateSaleCommand> UpdateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.Date, f => f.Date.Past(1))
        .RuleFor(c => c.CustomerId, f => f.Random.AlphaNumeric(24))
        .RuleFor(c => c.CustomerName, f => f.Name.FullName())
        .RuleFor(c => c.BranchId, f => Guid.NewGuid())
        .RuleFor(c => c.BranchName, f => f.Company.CompanyName());

    /// <summary>
    /// Configures the Faker to generate valid sale items.
    /// </summary>
    private static readonly Faker<UpdateSaleItemDto> UpdateSaleItemDtoFaker = new Faker<UpdateSaleItemDto>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(i => i.UnitPrice, f => decimal.Parse(f.Commerce.Price()));

    /// <summary>
    /// Generates a valid UpdateSaleCommand with random data.
    /// </summary>
    /// <param name="itemCount">Number of items to generate</param>
    /// <returns>A valid UpdateSaleCommand</returns>
    public static UpdateSaleCommand GenerateValidCommand(int itemCount = 3)
    {
        var command = UpdateSaleCommandFaker.Generate();
        command.Items = UpdateSaleItemDtoFaker.Generate(itemCount);
        return command;
    }

    /// <summary>
    /// Generates an UpdateSaleCommand with insufficient stock for a new item.
    /// </summary>
    /// <returns>An UpdateSaleCommand with an item quantity exceeding stock</returns>
    public static UpdateSaleCommand GenerateCommandWithInsufficientStock()
    {
        var command = GenerateValidCommand(1);
        command.Items[0].Quantity = 100;
        return command;
    }

    /// <summary>
    /// Generates an UpdateSaleCommand with invalid item quantity.
    /// </summary>
    /// <returns>An UpdateSaleCommand with an invalid item quantity</returns>
    public static UpdateSaleCommand GenerateCommandWithInvalidItemQuantity()
    {
        var command = GenerateValidCommand(1);
        command.Items[0].Quantity = 0;
        return command;
    }
}