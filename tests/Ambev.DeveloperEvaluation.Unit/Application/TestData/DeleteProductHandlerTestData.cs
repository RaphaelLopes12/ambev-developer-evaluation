using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for DeleteProductHandler tests.
/// </summary>
public static class DeleteProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid DeleteProductCommand instances.
    /// </summary>
    private static readonly Faker<DeleteProductCommand> DeleteProductCommandFaker = new Faker<DeleteProductCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid DeleteProductCommand with random data.
    /// </summary>
    /// <returns>A valid DeleteProductCommand</returns>
    public static DeleteProductCommand GenerateValidCommand()
    {
        return DeleteProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a DeleteProductCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>A DeleteProductCommand with the specified ID</returns>
    public static DeleteProductCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}