using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateProductHandler tests.
/// </summary>
public static class CreateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateProductCommand instances.
    /// </summary>
    private static readonly Faker<CreateProductCommand> CreateProductCommandFaker = new Faker<CreateProductCommand>()
        .RuleFor(c => c.Title, f => f.Commerce.ProductName())
        .RuleFor(c => c.Price, f => decimal.Parse(f.Commerce.Price()))
        .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
        .RuleFor(c => c.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(c => c.Image, f => f.Image.PicsumUrl())
        .RuleFor(c => c.StockQuantity, f => f.Random.Int(1, 100))
        .RuleFor(c => c.Rating, f => new RatingDto
        {
            Rate = f.Random.Decimal(0, 5),
            Count = f.Random.Int(0, 100)
        });

    /// <summary>
    /// Generates a valid CreateProductCommand with random data.
    /// </summary>
    /// <returns>A valid CreateProductCommand</returns>
    public static CreateProductCommand GenerateValidCommand()
    {
        return CreateProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateProductCommand with invalid price.
    /// </summary>
    /// <returns>A CreateProductCommand with invalid price</returns>
    public static CreateProductCommand GenerateCommandWithInvalidPrice()
    {
        var command = GenerateValidCommand();
        command.Price = 0;
        return command;
    }
}