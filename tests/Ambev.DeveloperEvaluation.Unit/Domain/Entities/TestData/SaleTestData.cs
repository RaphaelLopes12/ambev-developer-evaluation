using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// The generated sales will have valid:
    /// - Number (unique sale identifier)
    /// - Date (within reasonable past range)
    /// - CustomerId/CustomerName (valid customer references)
    /// - BranchId/BranchName (valid branch references)
    /// - Items (1-5 valid sale items)
    /// - Status (Active or Cancelled)
    /// </summary>
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale(
            number: f.Commerce.Ean13(),
            date: f.Date.Past(1),
            customerId: f.Random.Uuid().ToString(),
            customerName: f.Name.FullName(),
            branchId: Guid.NewGuid(),
            branchName: f.Company.CompanyName()
        ));

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        var sale = SaleFaker.Generate();

        // Add some valid sale items
        var faker = new Faker();
        var itemCount = faker.Random.Int(1, 5);

        for (int i = 0; i < itemCount; i++)
        {
            sale.AddItem(
                productId: Guid.NewGuid(),
                productName: faker.Commerce.ProductName(),
                quantity: faker.Random.Int(1, 10),
                unitPrice: decimal.Parse(faker.Commerce.Price())
            );
        }

        return sale;
    }

    /// <summary>
    /// Generates a valid sale number.
    /// </summary>
    /// <returns>A valid sale number.</returns>
    public static string GenerateValidSaleNumber()
    {
        return new Faker().Commerce.Ean13();
    }

    /// <summary>
    /// Generates an invalid sale number that exceeds maximum length.
    /// </summary>
    /// <returns>An invalid sale number.</returns>
    public static string GenerateInvalidSaleNumber()
    {
        return new string('X', 51); // Exceeds 50 character limit
    }

    /// <summary>
    /// Generates a sale with a future date (invalid).
    /// </summary>
    /// <returns>A sale with a future date.</returns>
    public static Sale GenerateSaleWithFutureDate()
    {
        var sale = SaleFaker.Generate();
        var futureDate = DateTime.UtcNow.AddDays(30);
        sale.UpdateDetails(
            futureDate,
            sale.CustomerId,
            sale.CustomerName,
            sale.BranchId,
            sale.BranchName
        );
        return sale;
    }

    /// <summary>
    /// Generates a cancelled sale for testing.
    /// </summary>
    /// <returns>A cancelled sale.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSale();
        sale.Cancel();
        return sale;
    }
}
