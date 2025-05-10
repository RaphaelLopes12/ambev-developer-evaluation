using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for UpdateBranchHandler tests.
/// </summary>
public static class UpdateBranchHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateBranchCommand instances.
    /// </summary>
    private static readonly Faker<UpdateBranchCommand> UpdateBranchCommandFaker = new Faker<UpdateBranchCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.Name, f => f.Company.CompanyName())
        .RuleFor(c => c.Address, f => f.Address.StreetAddress())
        .RuleFor(c => c.City, f => f.Address.City())
        .RuleFor(c => c.State, f => f.Address.State())
        .RuleFor(c => c.ZipCode, f => f.Address.ZipCode())
        .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"))
        .RuleFor(c => c.Email, f => f.Internet.Email());

    /// <summary>
    /// Generates a valid UpdateBranchCommand with random data.
    /// </summary>
    /// <returns>A valid UpdateBranchCommand</returns>
    public static UpdateBranchCommand GenerateValidCommand()
    {
        return UpdateBranchCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an UpdateBranchCommand with an invalid email.
    /// </summary>
    /// <returns>An UpdateBranchCommand with an invalid email</returns>
    public static UpdateBranchCommand GenerateCommandWithInvalidEmail()
    {
        var command = GenerateValidCommand();
        command.Email = "invalid-email";
        return command;
    }

    /// <summary>
    /// Generates an UpdateBranchCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use in the command</param>
    /// <returns>An UpdateBranchCommand with the specified ID</returns>
    public static UpdateBranchCommand GenerateCommandWithId(Guid id)
    {
        var command = GenerateValidCommand();
        command.Id = id;
        return command;
    }
}