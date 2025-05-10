using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for SaleItem entities.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Configures the Faker to generate valid SaleItem entities.
    /// </summary>
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .CustomInstantiator(f => new SaleItem(
            productId: Guid.NewGuid(),
            productName: f.Commerce.ProductName(),
            quantity: f.Random.Int(1, 10),
            unitPrice: decimal.Parse(f.Commerce.Price())
        ));

    /// <summary>
    /// Generates a valid SaleItem entity with randomized data.
    /// </summary>
    /// <returns>A valid SaleItem with randomly generated data.</returns>
    public static SaleItem GenerateValidSaleItem()
    {
        return SaleItemFaker.Generate();
    }

    /// <summary>
    /// Generates a SaleItem with excessive quantity (invalid).
    /// </summary>
    /// <returns>A SaleItem with excessive quantity.</returns>
    public static SaleItem GenerateSaleItemWithExcessiveQuantity()
    {
        var faker = new Faker();
        return new SaleItem(
            productId: Guid.NewGuid(),
            productName: faker.Commerce.ProductName(),
            quantity: 21, // Exceeds maximum quantity of 20
            unitPrice: decimal.Parse(faker.Commerce.Price())
        );
    }

    /// <summary>
    /// Generates a cancelled SaleItem for testing.
    /// </summary>
    /// <returns>A cancelled SaleItem.</returns>
    public static SaleItem GenerateCancelledSaleItem()
    {
        var item = SaleItemFaker.Generate();
        item.Cancel();
        return item;
    }
}
