using Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for ActivateBranchHandler tests.
/// </summary>
public static class ActivateBranchHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid ActivateBranchCommand instances.
    /// </summary>
    private static readonly Faker<ActivateBranchCommand> ActivateBranchCommandFaker = new Faker<ActivateBranchCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid ActivateBranchCommand with random data.
    /// </summary>
    /// <returns>A valid ActivateBranchCommand</returns>
    public static ActivateBranchCommand GenerateValidCommand()
    {
        return ActivateBranchCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an ActivateBranchCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>An ActivateBranchCommand with the specified ID</returns>
    public static ActivateBranchCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}