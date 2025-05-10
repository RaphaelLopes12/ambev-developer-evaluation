using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using System;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Product entity class.
    /// Tests cover business rules, validation, and behavior.
    /// </summary>
    public class ProductTests
    {
        /// <summary>
        /// Tests that a product can be created with valid data.
        /// </summary>
        [Fact(DisplayName = "Product with valid data should be created successfully")]
        public void Given_ValidData_When_CreatingProduct_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string title = "Test Product";
            decimal price = 19.99m;
            string description = "Test Description";
            string category = "Test Category";
            string image = "test-image.jpg";
            int stockQuantity = 50;

            // Act
            var product = new Product(title, price, description, category, image, stockQuantity);

            // Assert
            product.Title.Should().Be(title);
            product.Price.Should().Be(price);
            product.Description.Should().Be(description);
            product.Category.Should().Be(category);
            product.Image.Should().Be(image);
            product.StockQuantity.Should().Be(stockQuantity);
            product.RatingRate.Should().Be(0);
            product.RatingCount.Should().Be(0);
            product.Id.Should().NotBe(Guid.Empty);
        }

        /// <summary>
        /// Tests that product details can be updated.
        /// </summary>
        [Fact(DisplayName = "UpdateDetails should update product properties")]
        public void Given_Product_When_UpdatingDetails_Then_PropertiesShouldUpdate()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();
            string newTitle = "Updated Title";
            decimal newPrice = 29.99m;
            string newDescription = "Updated Description";
            string newCategory = "Updated Category";
            string newImage = "updated-image.jpg";

            // Act
            product.UpdateDetails(newTitle, newPrice, newDescription, newCategory, newImage);

            // Assert
            product.Title.Should().Be(newTitle);
            product.Price.Should().Be(newPrice);
            product.Description.Should().Be(newDescription);
            product.Category.Should().Be(newCategory);
            product.Image.Should().Be(newImage);
            product.UpdatedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that stock quantity can be updated.
        /// </summary>
        [Fact(DisplayName = "UpdateStock should update stock quantity")]
        public void Given_Product_When_UpdatingStock_Then_StockQuantityShouldUpdate()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();
            int newQuantity = 100;

            // Act
            product.UpdateStock(newQuantity);

            // Assert
            product.StockQuantity.Should().Be(newQuantity);
            product.UpdatedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that stock quantity cannot be set to a negative value.
        /// </summary>
        [Fact(DisplayName = "UpdateStock with negative quantity should throw exception")]
        public void Given_Product_When_UpdatingStockWithNegativeQuantity_Then_ShouldThrowException()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();
            int negativeQuantity = -10;

            // Act
            Action act = () => product.UpdateStock(negativeQuantity);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Stock quantity cannot be negative.*");
        }

        /// <summary>
        /// Tests that stock quantity can be decreased.
        /// </summary>
        [Fact(DisplayName = "DecreaseStock should subtract from stock quantity")]
        public void Given_Product_When_DecreasingStock_Then_StockQuantityShouldDecrease()
        {
            // Arrange
            int initialStock = 50;
            var product = ProductTestData.GenerateProductWithStock(initialStock);
            int decreaseAmount = 10;

            // Act
            product.DecreaseStock(decreaseAmount);

            // Assert
            product.StockQuantity.Should().Be(initialStock - decreaseAmount);
            product.UpdatedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that stock quantity cannot be decreased by more than available.
        /// </summary>
        [Fact(DisplayName = "DecreaseStock with amount greater than stock should throw exception")]
        public void Given_Product_When_DecreasingStockBeyondAvailable_Then_ShouldThrowException()
        {
            // Arrange
            int initialStock = 10;
            var product = ProductTestData.GenerateProductWithStock(initialStock);
            int decreaseAmount = 20;

            // Act
            Action act = () => product.DecreaseStock(decreaseAmount);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Not enough items in stock.");
        }

        /// <summary>
        /// Tests that stock quantity can be increased.
        /// </summary>
        [Fact(DisplayName = "IncreaseStock should add to stock quantity")]
        public void Given_Product_When_IncreasingStock_Then_StockQuantityShouldIncrease()
        {
            // Arrange
            int initialStock = 50;
            var product = ProductTestData.GenerateProductWithStock(initialStock);
            int increaseAmount = 10;

            // Act
            product.IncreaseStock(increaseAmount);

            // Assert
            product.StockQuantity.Should().Be(initialStock + increaseAmount);
            product.UpdatedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that a rating can be added to a product.
        /// </summary>
        [Fact(DisplayName = "AddRating should update rating average and count")]
        public void Given_Product_When_AddingRating_Then_RatingAverageAndCountShouldUpdate()
        {
            // Arrange
            var product = ProductTestData.GenerateProductWithStock(50);
            // Reset ratings to zero
            typeof(Product).GetProperty("RatingRate").SetValue(product, 0m);
            typeof(Product).GetProperty("RatingCount").SetValue(product, 0);
            decimal rating = 5m;

            // Act
            product.AddRating(rating);

            // Assert
            product.RatingCount.Should().Be(1);
            product.RatingRate.Should().Be(rating);
            product.UpdatedAt.Should().NotBeNull();

            // Add another rating
            product.AddRating(3.5m);

            // Check average is calculated correctly
            product.RatingCount.Should().Be(2);
            product.RatingRate.Should().Be(4.25m); // (5.0 + 3.5) / 2 = 4.25
        }

        /// <summary>
        /// Tests that validation passes for a valid product.
        /// </summary>
        [Fact(DisplayName = "Valid product should pass validation")]
        public void Given_ValidProduct_When_Validated_Then_ShouldReturnValid()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();

            // Act
            var result = product.Validate();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that validation fails for a product with invalid price.
        /// </summary>
        [Fact(DisplayName = "Product with invalid price should fail validation")]
        public void Given_ProductWithInvalidPrice_When_Validated_Then_ShouldReturnInvalid()
        {
            // Arrange
            var product = ProductTestData.GenerateProductWithInvalidPrice();

            // Act
            var result = product.Validate();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Detail.Contains("Price must be greater than zero"));
        }
    }
}