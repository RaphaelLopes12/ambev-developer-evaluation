using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Branch entity.
/// </summary>
public static class BranchTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Branch instances.
    /// </summary>
    private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
        .CustomInstantiator(f => new Branch(
            name: f.Company.CompanyName(),
            address: f.Address.StreetAddress(),
            city: f.Address.City(),
            state: f.Address.State(),
            zipCode: f.Address.ZipCode(),
            phone: f.Phone.PhoneNumber("###-###-####"),
            email: f.Internet.Email()
        ));

    /// <summary>
    /// Generates a valid Branch with random data.
    /// </summary>
    /// <returns>A valid Branch</returns>
    public static Branch GenerateValidBranch()
    {
        return BranchFaker.Generate();
    }

    /// <summary>
    /// Generates a Branch with a specific ID.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <returns>A Branch with the specified ID</returns>
    public static Branch GenerateBranchWithId(Guid id)
    {
        var branch = GenerateValidBranch();
        typeof(Branch).GetProperty("Id").SetValue(branch, id);
        return branch;
    }

    /// <summary>
    /// Generates a Branch that is inactive.
    /// </summary>
    /// <returns>An inactive Branch</returns>
    public static Branch GenerateInactiveBranch()
    {
        var branch = GenerateValidBranch();
        branch.Deactivate();
        return branch;
    }

    /// <summary>
    /// Generates a list of Branches with random data.
    /// </summary>
    /// <param name="count">The number of Branches to generate</param>
    /// <returns>A list of valid Branches</returns>
    public static List<Branch> GenerateValidBranches(int count)
    {
        return BranchFaker.Generate(count);
    }
}
