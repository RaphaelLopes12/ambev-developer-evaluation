using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover business rules, validation, and behavior.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a sale can be created with valid data.
    /// </summary>
    [Fact(DisplayName = "Sale with valid data should be created successfully")]
    public void Given_ValidData_When_CreatingSale_Then_ShouldCreateSuccessfully()
    {
        // Arrange
        var number = SaleTestData.GenerateValidSaleNumber();
        var date = DateTime.UtcNow.AddDays(-1);
        var customerId = Guid.NewGuid().ToString();
        var customerName = "Test Customer";
        var branchId = Guid.NewGuid();
        var branchName = "Test Branch";

        // Act
        var sale = new Sale(number, date, customerId, customerName, branchId, branchName);

        // Assert
        sale.Number.Should().Be(number);
        sale.Date.Should().Be(date);
        sale.CustomerId.Should().Be(customerId);
        sale.CustomerName.Should().Be(customerName);
        sale.BranchId.Should().Be(branchId);
        sale.BranchName.Should().Be(branchName);
        sale.Status.Should().Be(SaleStatus.Active);
        sale.Items.Should().BeEmpty();
        sale.TotalAmount.Should().Be(0);
    }

    /// <summary>
    /// Tests that items can be added to a sale.
    /// </summary>
    [Fact(DisplayName = "Adding item to sale should increase total amount")]
    public void Given_Sale_When_AddingItem_Then_TotalAmountShouldIncrease()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 2;
        var unitPrice = 10.5m;

        // Act
        sale.AddItem(productId, productName, quantity, unitPrice);

        // Assert
        sale.Items.Should().Contain(i => i.ProductId == productId);
        sale.TotalAmount.Should().Be(sale.Items.Sum(i => i.Total));
    }

    /// <summary>
    /// Tests that a sale with a duplicate product ID throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding duplicate product should throw exception")]
    public void Given_SaleWithProduct_When_AddingDuplicateProduct_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = sale.Items.First();

        // Act
        Action act = () => sale.AddItem(product.ProductId, "Duplicate Product", 1, 10m);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"*already exists in this sale*");
    }

    /// <summary>
    /// Tests that a sale item can be updated.
    /// </summary>
    [Fact(DisplayName = "Updating item quantity should recalculate total")]
    public void Given_SaleWithItem_When_UpdatingQuantity_Then_TotalShouldBeRecalculated()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var item = sale.Items.First();
        var originalTotal = sale.TotalAmount;
        var newQuantity = item.Quantity + 2;

        // Act
        sale.UpdateItem(item.ProductId, newQuantity);

        // Assert
        var updatedItem = sale.Items.First(i => i.ProductId == item.ProductId);
        updatedItem.Quantity.Should().Be(newQuantity);
        sale.TotalAmount.Should().NotBe(originalTotal);
    }

    /// <summary>
    /// Tests that a sale can be cancelled.
    /// </summary>
    [Fact(DisplayName = "Cancelling sale should change status and cancel all items")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeChangedAndItemsCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.Cancel();

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
        sale.TotalAmount.Should().Be(0);
    }

    /// <summary>
    /// Tests that cancelling an already cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Cancelling already cancelled sale should throw exception")]
    public void Given_CancelledSale_When_Cancelling_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();

        // Act
        Action act = () => sale.Cancel();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*already cancelled*");
    }

    /// <summary>
    /// Tests that a specific item in a sale can be cancelled.
    /// </summary>
    [Fact(DisplayName = "Cancelling item should mark it as cancelled and adjust total")]
    public void Given_SaleWithItems_When_CancellingItem_Then_ItemShouldBeMarkedAsCancelledAndTotalAdjusted()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var item = sale.Items.First();
        var originalTotal = sale.TotalAmount;

        // Act
        sale.CancelItem(item.ProductId);

        // Assert
        item.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().BeLessThan(originalTotal);
    }

    /// <summary>
    /// Tests that validation passes for a valid sale.
    /// </summary>
    [Fact(DisplayName = "Valid sale should pass validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation fails for a sale with a future date.
    /// </summary>
    [Fact(DisplayName = "Sale with future date should fail validation")]
    public void Given_SaleWithFutureDate_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithFutureDate();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("future"));
    }
}
