using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Branch entity.
/// </summary>
public class BranchTests
{
    /// <summary>
    /// Tests that a branch is created with valid properties.
    /// </summary>
    [Fact(DisplayName = "New branch should have valid properties")]
    public void Given_NewBranch_When_Created_Then_ShouldHaveValidProperties()
    {
        // Arrange
        string name = "Headquarters";
        string address = "123 Main St";
        string city = "New York";
        string state = "NY";
        string zipCode = "10001";
        string phone = "555-123-4567";
        string email = "info@company.com";

        // Act
        var branch = new Branch(name, address, city, state, zipCode, phone, email);

        // Assert
        branch.Name.Should().Be(name);
        branch.Address.Should().Be(address);
        branch.City.Should().Be(city);
        branch.State.Should().Be(state);
        branch.ZipCode.Should().Be(zipCode);
        branch.Phone.Should().Be(phone);
        branch.Email.Should().Be(email);
        branch.IsActive.Should().BeTrue();
        branch.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        branch.UpdatedAt.Should().BeNull();
        branch.Id.Should().NotBe(Guid.Empty);
    }

    /// <summary>
    /// Tests that a branch's details can be updated.
    /// </summary>
    [Fact(DisplayName = "UpdateDetails should update branch properties")]
    public void Given_Branch_When_UpdateDetails_Then_PropertiesShouldBeUpdated()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        string newName = "New Headquarters";
        string newAddress = "456 Business Ave";
        string newCity = "Boston";
        string newState = "MA";
        string newZipCode = "02108";
        string newPhone = "555-987-6543";
        string newEmail = "new-info@company.com";

        // Act
        branch.UpdateDetails(newName, newAddress, newCity, newState, newZipCode, newPhone, newEmail);

        // Assert
        branch.Name.Should().Be(newName);
        branch.Address.Should().Be(newAddress);
        branch.City.Should().Be(newCity);
        branch.State.Should().Be(newState);
        branch.ZipCode.Should().Be(newZipCode);
        branch.Phone.Should().Be(newPhone);
        branch.Email.Should().Be(newEmail);
        branch.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that a branch can be activated.
    /// </summary>
    [Fact(DisplayName = "Activate should set IsActive to true")]
    public void Given_InactiveBranch_When_Activate_Then_ShouldBeActive()
    {
        // Arrange
        var branch = BranchTestData.GenerateInactiveBranch();

        // Act
        branch.Activate();

        // Assert
        branch.IsActive.Should().BeTrue();
        branch.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that activating an already active branch throws an exception.
    /// </summary>
    [Fact(DisplayName = "Activating an already active branch should throw exception")]
    public void Given_ActiveBranch_When_Activate_Then_ShouldThrowException()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act & Assert
        Action act = () => branch.Activate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Branch is already active.");
    }

    /// <summary>
    /// Tests that a branch can be deactivated.
    /// </summary>
    [Fact(DisplayName = "Deactivate should set IsActive to false")]
    public void Given_ActiveBranch_When_Deactivate_Then_ShouldBeInactive()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        branch.Deactivate();

        // Assert
        branch.IsActive.Should().BeFalse();
        branch.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that deactivating an already inactive branch throws an exception.
    /// </summary>
    [Fact(DisplayName = "Deactivating an already inactive branch should throw exception")]
    public void Given_InactiveBranch_When_Deactivate_Then_ShouldThrowException()
    {
        // Arrange
        var branch = BranchTestData.GenerateInactiveBranch();

        // Act & Assert
        Action act = () => branch.Deactivate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Branch is already inactive.");
    }

    /// <summary>
    /// Tests validation of a valid branch.
    /// </summary>
    [Fact(DisplayName = "Valid branch should pass validation")]
    public void Given_ValidBranch_When_Validate_Then_ShouldBeValid()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests validation of a branch with an invalid email.
    /// </summary>
    [Fact(DisplayName = "Branch with invalid email should fail validation")]
    public void Given_BranchWithInvalidEmail_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        typeof(Branch).GetProperty("Email").SetValue(branch, "invalid-email");

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("valid email"));
    }

    /// <summary>
    /// Tests validation of a branch with an empty name.
    /// </summary>
    [Fact(DisplayName = "Branch with empty name should fail validation")]
    public void Given_BranchWithEmptyName_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        typeof(Branch).GetProperty("Name").SetValue(branch, string.Empty);

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Name is required"));
    }
}