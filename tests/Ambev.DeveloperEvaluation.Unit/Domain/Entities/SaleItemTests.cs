using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover discount calculations, validation, and behavior.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that a sale item can be created with valid data.
    /// </summary>
    [Fact(DisplayName = "SaleItem with valid data should be created successfully")]
    public void Given_ValidData_When_CreatingSaleItem_Then_ShouldCreateSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 2;
        var unitPrice = 10.5m;

        // Act
        var saleItem = new SaleItem(productId, productName, quantity, unitPrice);

        // Assert
        saleItem.ProductId.Should().Be(productId);
        saleItem.ProductName.Should().Be(productName);
        saleItem.Quantity.Should().Be(quantity);
        saleItem.UnitPrice.Should().Be(unitPrice);
        saleItem.IsCancelled.Should().BeFalse();
        saleItem.Total.Should().Be(quantity * unitPrice - saleItem.Discount);
    }

    /// <summary>
    /// Tests that discount is calculated correctly based on quantity.
    /// </summary>
    [Theory(DisplayName = "Discount should be calculated correctly based on quantity")]
    [InlineData(1, 10, 0)] // No discount for 1-3 items
    [InlineData(3, 10, 0)] // No discount for 1-3 items
    [InlineData(4, 10, 4)] // 10% discount for 4-9 items (4 * 10 * 0.1 = 4)
    [InlineData(9, 10, 9)] // 10% discount for 4-9 items (9 * 10 * 0.1 = 9)
    [InlineData(10, 10, 20)] // 20% discount for 10-20 items (10 * 10 * 0.2 = 20)
    [InlineData(20, 10, 40)] // 20% discount for 10-20 items (20 * 10 * 0.2 = 40)
    public void Given_QuantityAndPrice_When_CalculatingDiscount_Then_ShouldBeCalculatedCorrectly(int quantity, decimal unitPrice, decimal expectedDiscount)
    {
        // Act
        var saleItem = new SaleItem(Guid.NewGuid(), "Test Product", quantity, unitPrice);

        // Assert
        saleItem.Discount.Should().Be(expectedDiscount);
    }

    /// <summary>
    /// Tests that creating a sale item with excessive quantity throws an exception.
    /// </summary>
    [Fact(DisplayName = "Creating SaleItem with excessive quantity should throw exception")]
    public void Given_ExcessiveQuantity_When_CreatingSaleItem_Then_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 21; // Exceeds maximum of 20
        var unitPrice = 10.5m;

        // Act
        Action act = () => new SaleItem(productId, productName, quantity, unitPrice);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Cannot sell more than 20 items*");
    }

    /// <summary>
    /// Tests that a sale item can be cancelled.
    /// </summary>
    [Fact(DisplayName = "Cancelling SaleItem should mark it as cancelled and set total to zero")]
    public void Given_ActiveSaleItem_When_Cancelled_Then_ShouldBeMarkedAsCancelledAndTotalSetToZero()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var originalTotal = saleItem.Total;

        // Act
        saleItem.Cancel();

        // Assert
        saleItem.IsCancelled.Should().BeTrue();
        saleItem.Total.Should().Be(0);
        originalTotal.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Tests that quantity can be updated.
    /// </summary>
    [Fact(DisplayName = "Updating quantity should recalculate total")]
    public void Given_SaleItem_When_UpdatingQuantity_Then_TotalShouldBeRecalculated()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var originalTotal = saleItem.Total;
        var newQuantity = saleItem.Quantity + 2;
        var newDiscount = 0m;

        // Calculate expected discount based on new quantity
        if (newQuantity >= 10)
            newDiscount = newQuantity * saleItem.UnitPrice * 0.20m;
        else if (newQuantity >= 4)
            newDiscount = newQuantity * saleItem.UnitPrice * 0.10m;

        // Act
        saleItem.UpdateQuantity(newQuantity, newDiscount);

        // Assert
        saleItem.Quantity.Should().Be(newQuantity);
        saleItem.Discount.Should().Be(newDiscount);
        saleItem.Total.Should().Be(newQuantity * saleItem.UnitPrice - newDiscount);
        saleItem.Total.Should().NotBe(originalTotal);
    }

    /// <summary>
    /// Tests that validation passes for a valid sale item.
    /// </summary>
    [Fact(DisplayName = "Valid SaleItem should pass validation")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
