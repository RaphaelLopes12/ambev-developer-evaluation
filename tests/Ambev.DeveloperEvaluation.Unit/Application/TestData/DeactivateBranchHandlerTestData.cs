using Ambev.DeveloperEvaluation.Application.Branches.DeactivateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for DeactivateBranchHandler tests.
/// </summary>
public static class DeactivateBranchHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid DeactivateBranchCommand instances.
    /// </summary>
    private static readonly Faker<DeactivateBranchCommand> DeactivateBranchCommandFaker = new Faker<DeactivateBranchCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid());

    /// <summary>
    /// Generates a valid DeactivateBranchCommand with random data.
    /// </summary>
    /// <returns>A valid DeactivateBranchCommand</returns>
    public static DeactivateBranchCommand GenerateValidCommand()
    {
        return DeactivateBranchCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a DeactivateBranchCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>A DeactivateBranchCommand with the specified ID</returns>
    public static DeactivateBranchCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}