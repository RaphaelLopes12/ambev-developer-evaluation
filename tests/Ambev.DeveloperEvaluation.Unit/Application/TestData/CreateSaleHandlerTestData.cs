using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler tests.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand instances.
    /// </summary>
    private static readonly Faker<CreateSaleCommand> CreateSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(c => c.Number, f => f.Commerce.Ean13())
        .RuleFor(c => c.Date, f => f.Date.Past(1))
        .RuleFor(c => c.CustomerId, f => f.Random.AlphaNumeric(24))
        .RuleFor(c => c.CustomerName, f => f.Name.FullName())
        .RuleFor(c => c.BranchId, f => Guid.NewGuid())
        .RuleFor(c => c.BranchName, f => f.Company.CompanyName());

    /// <summary>
    /// Configures the Faker to generate valid sale items.
    /// </summary>
    private static readonly Faker<CreateSaleItemDto> CreateSaleItemDtoFaker = new Faker<CreateSaleItemDto>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(i => i.UnitPrice, f => decimal.Parse(f.Commerce.Price()));

    /// <summary>
    /// Generates a valid CreateSaleCommand with random data.
    /// </summary>
    /// <param name="itemCount">Number of items to generate</param>
    /// <returns>A valid CreateSaleCommand</returns>
    public static CreateSaleCommand GenerateValidCommand(int itemCount = 3)
    {
        var command = CreateSaleCommandFaker.Generate();
        command.Items = CreateSaleItemDtoFaker.Generate(itemCount);
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with insufficient stock.
    /// </summary>
    /// <returns>A CreateSaleCommand with an item quantity exceeding stock</returns>
    public static CreateSaleCommand GenerateCommandWithInsufficientStock()
    {
        var command = GenerateValidCommand(1);
        command.Items[0].Quantity = 100; // Setting high quantity to simulate insufficient stock
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with invalid item quantity.
    /// </summary>
    /// <returns>A CreateSaleCommand with an invalid item quantity</returns>
    public static CreateSaleCommand GenerateCommandWithInvalidItemQuantity()
    {
        var command = GenerateValidCommand(1);
        command.Items[0].Quantity = 0; // Invalid quantity
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with duplicate product IDs.
    /// </summary>
    /// <returns>A CreateSaleCommand with duplicate product IDs</returns>
    public static CreateSaleCommand GenerateCommandWithDuplicateProducts()
    {
        var command = GenerateValidCommand(1);
        var duplicateItem = new CreateSaleItemDto
        {
            ProductId = command.Items[0].ProductId,
            ProductName = command.Items[0].ProductName,
            Quantity = 2,
            UnitPrice = command.Items[0].UnitPrice
        };
        command.Items.Add(duplicateItem);
        return command;
    }
}
