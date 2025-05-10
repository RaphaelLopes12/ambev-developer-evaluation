using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for DeleteBranchHandler tests.
/// </summary>
public static class DeleteBranchHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid DeleteBranchCommand instances.
    /// </summary>
    private static readonly Faker<DeleteBranchCommand> DeleteBranchCommandFaker = new Faker<DeleteBranchCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid DeleteBranchCommand with random data.
    /// </summary>
    /// <returns>A valid DeleteBranchCommand</returns>
    public static DeleteBranchCommand GenerateValidCommand()
    {
        return DeleteBranchCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a DeleteBranchCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>A DeleteBranchCommand with the specified ID</returns>
    public static DeleteBranchCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}