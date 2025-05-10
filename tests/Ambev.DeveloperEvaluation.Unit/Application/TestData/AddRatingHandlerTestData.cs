using Ambev.DeveloperEvaluation.Application.Products.AddRating;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for AddRatingHandler tests.
/// </summary>
public static class AddRatingHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid AddRatingCommand instances.
    /// </summary>
    private static readonly Faker<AddRatingCommand> AddRatingCommandFaker = new Faker<AddRatingCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.Rating, f => f.Random.Decimal(1, 5));

    /// <summary>
    /// Generates a valid AddRatingCommand with random data.
    /// </summary>
    /// <returns>A valid AddRatingCommand</returns>
    public static AddRatingCommand GenerateValidCommand()
    {
        return AddRatingCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an AddRatingCommand with a rating outside the valid range.
    /// </summary>
    /// <returns>An AddRatingCommand with invalid rating</returns>
    public static AddRatingCommand GenerateCommandWithInvalidRating()
    {
        var command = GenerateValidCommand();
        command.Rating = 6; // Invalid: greater than 5
        return command;
    }

    /// <summary>
    /// Generates an AddRatingCommand with a specific product ID.
    /// </summary>
    /// <param name="productId">The product ID to use</param>
    /// <returns>An AddRatingCommand with the specified product ID</returns>
    public static AddRatingCommand GenerateCommandWithProductId(Guid productId)
    {
        var command = GenerateValidCommand();
        command.Id = productId;
        return command;
    }
}