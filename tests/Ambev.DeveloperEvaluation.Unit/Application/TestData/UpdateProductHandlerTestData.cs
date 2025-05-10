using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for UpdateProductHandler tests.
/// </summary>
public static class UpdateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateProductCommand instances.
    /// </summary>
    private static readonly Faker<UpdateProductCommand> UpdateProductCommandFaker = new Faker<UpdateProductCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.Title, f => f.Commerce.ProductName())
        .RuleFor(c => c.Price, f => decimal.Parse(f.Commerce.Price()))
        .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
        .RuleFor(c => c.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(c => c.Image, f => f.Image.PicsumUrl());

    /// <summary>
    /// Generates a valid UpdateProductCommand with random data.
    /// </summary>
    /// <returns>A valid UpdateProductCommand</returns>
    public static UpdateProductCommand GenerateValidCommand()
    {
        return UpdateProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an UpdateProductCommand with invalid price.
    /// </summary>
    /// <returns>An UpdateProductCommand with invalid price</returns>
    public static UpdateProductCommand GenerateCommandWithInvalidPrice()
    {
        var command = GenerateValidCommand();
        command.Price = 0;
        return command;
    }
}