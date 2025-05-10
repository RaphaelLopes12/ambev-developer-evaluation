using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Customer entity.
/// </summary>
public class CustomerTests
{
    /// <summary>
    /// Tests that a customer is created with valid properties.
    /// </summary>
    [Fact(DisplayName = "New customer should have valid properties")]
    public void Given_NewCustomer_When_Created_Then_ShouldHaveValidProperties()
    {
        // Arrange
        string name = "John Doe";
        string email = "john.doe@example.com";
        string phone = "555-123-4567";
        string address = "123 Main St, Anytown, USA";

        // Act
        var customer = new Customer(name, email, phone, address);

        // Assert
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
        customer.Phone.Should().Be(phone);
        customer.Address.Should().Be(address);
        customer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        customer.UpdatedAt.Should().BeNull();
        customer.PostgreSQLId.Should().BeNull();
    }

    /// <summary>
    /// Tests that a customer's details can be updated.
    /// </summary>
    [Fact(DisplayName = "UpdateDetails should update customer properties")]
    public void Given_Customer_When_UpdateDetails_Then_PropertiesShouldBeUpdated()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        string newName = "Jane Smith";
        string newEmail = "jane.smith@example.com";
        string newPhone = "555-987-6543";
        string newAddress = "456 Park Ave, Othertown, USA";

        // Act
        customer.UpdateDetails(newName, newEmail, newPhone, newAddress);

        // Assert
        customer.Name.Should().Be(newName);
        customer.Email.Should().Be(newEmail);
        customer.Phone.Should().Be(newPhone);
        customer.Address.Should().Be(newAddress);
        customer.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that a PostgreSQL ID can be set.
    /// </summary>
    [Fact(DisplayName = "SetPostgreSQLId should update PostgreSQLId")]
    public void Given_Customer_When_SetPostgreSQLId_Then_PostgreSQLIdShouldBeUpdated()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        int pgId = 42;

        // Act
        customer.SetPostgreSQLId(pgId);

        // Assert
        customer.PostgreSQLId.Should().Be(pgId);
        customer.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests validation of a valid customer.
    /// </summary>
    [Fact(DisplayName = "Valid customer should pass validation")]
    public void Given_ValidCustomer_When_Validate_Then_ShouldBeValid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests validation of a customer with an invalid email.
    /// </summary>
    [Fact(DisplayName = "Customer with invalid email should fail validation")]
    public void Given_CustomerWithInvalidEmail_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        typeof(Customer).GetProperty("Email").SetValue(customer, "invalid-email");

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("email"));
    }

    /// <summary>
    /// Tests validation of a customer with an empty name.
    /// </summary>
    [Fact(DisplayName = "Customer with empty name should fail validation")]
    public void Given_CustomerWithEmptyName_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        typeof(Customer).GetProperty("Name").SetValue(customer, string.Empty);

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Name is required"));
    }

    /// <summary>
    /// Tests validation of a customer with a name that exceeds the maximum length.
    /// </summary>
    [Fact(DisplayName = "Customer with name exceeding maximum length should fail validation")]
    public void Given_CustomerWithNameExceedingMaxLength_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        string longName = new string('A', 101); // 101 characters, max is 100
        typeof(Customer).GetProperty("Name").SetValue(customer, longName);

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("cannot exceed 100 characters"));
    }

    /// <summary>
    /// Tests validation of a customer with an empty phone number.
    /// </summary>
    [Fact(DisplayName = "Customer with empty phone should fail validation")]
    public void Given_CustomerWithEmptyPhone_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        typeof(Customer).GetProperty("Phone").SetValue(customer, string.Empty);

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Phone number is required"));
    }

    /// <summary>
    /// Tests validation of a customer with an empty address.
    /// </summary>
    [Fact(DisplayName = "Customer with empty address should fail validation")]
    public void Given_CustomerWithEmptyAddress_When_Validate_Then_ShouldBeInvalid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        typeof(Customer).GetProperty("Address").SetValue(customer, string.Empty);

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Address is required"));
    }
}