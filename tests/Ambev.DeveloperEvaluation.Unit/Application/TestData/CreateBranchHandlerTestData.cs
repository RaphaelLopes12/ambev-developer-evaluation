using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateBranchHandler tests.
/// </summary>
public static class CreateBranchHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateBranchCommand instances.
    /// </summary>
    private static readonly Faker<CreateBranchCommand> CreateBranchCommandFaker = new Faker<CreateBranchCommand>()
        .RuleFor(c => c.Name, f => f.Company.CompanyName())
        .RuleFor(c => c.Address, f => f.Address.StreetAddress())
        .RuleFor(c => c.City, f => f.Address.City())
        .RuleFor(c => c.State, f => f.Address.State())
        .RuleFor(c => c.ZipCode, f => f.Address.ZipCode())
        .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"))
        .RuleFor(c => c.Email, f => f.Internet.Email());

    /// <summary>
    /// Generates a valid CreateBranchCommand with random data.
    /// </summary>
    /// <returns>A valid CreateBranchCommand</returns>
    public static CreateBranchCommand GenerateValidCommand()
    {
        return CreateBranchCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateBranchCommand with an invalid email.
    /// </summary>
    /// <returns>A CreateBranchCommand with an invalid email</returns>
    public static CreateBranchCommand GenerateCommandWithInvalidEmail()
    {
        var command = GenerateValidCommand();
        command.Email = "invalid-email";
        return command;
    }

    /// <summary>
    /// Generates a CreateBranchCommand with an empty name.
    /// </summary>
    /// <returns>A CreateBranchCommand with an empty name</returns>
    public static CreateBranchCommand GenerateCommandWithEmptyName()
    {
        var command = GenerateValidCommand();
        command.Name = string.Empty;
        return command;
    }
}