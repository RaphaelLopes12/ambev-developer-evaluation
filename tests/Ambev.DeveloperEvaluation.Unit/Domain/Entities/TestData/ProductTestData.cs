using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class ProductTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid Product entities.
        /// </summary>
        private static readonly Faker<Product> ProductFaker = new Faker<Product>()
            .CustomInstantiator(f => new Product(
                title: f.Commerce.ProductName(),
                price: decimal.Parse(f.Commerce.Price()),
                description: f.Commerce.ProductDescription(),
                category: f.Commerce.Categories(1)[0],
                image: f.Image.PicsumUrl(),
                stockQuantity: f.Random.Int(1, 100),
                ratingRate: f.Random.Decimal(0, 5),
                ratingCount: f.Random.Int(0, 100)
            ));

        /// <summary>
        /// Generates a valid Product entity with randomized data.
        /// </summary>
        /// <returns>A valid Product entity with randomly generated data.</returns>
        public static Product GenerateValidProduct()
        {
            return ProductFaker.Generate();
        }

        /// <summary>
        /// Generates a product with a specific ID.
        /// </summary>
        /// <param name="id">The ID to set for the product.</param>
        /// <returns>A product with the specified ID.</returns>
        public static Product GenerateProductWithId(Guid id)
        {
            var product = GenerateValidProduct();
            SetPrivateProperty(product, "Id", id);
            return product;
        }

        /// <summary>
        /// Generates a product with a specific stock quantity.
        /// </summary>
        /// <param name="stockQuantity">The stock quantity to set.</param>
        /// <returns>A product with the specified stock quantity.</returns>
        public static Product GenerateProductWithStock(int stockQuantity)
        {
            var faker = new Faker();
            return new Product(
                title: faker.Commerce.ProductName(),
                price: decimal.Parse(faker.Commerce.Price()),
                description: faker.Commerce.ProductDescription(),
                category: faker.Commerce.Categories(1)[0],
                image: faker.Image.PicsumUrl(),
                stockQuantity: stockQuantity,
                ratingRate: faker.Random.Decimal(0, 5),
                ratingCount: faker.Random.Int(0, 100)
            );
        }

        /// <summary>
        /// Generates a product with invalid price (zero or negative).
        /// </summary>
        /// <returns>A product with invalid price.</returns>
        public static Product GenerateProductWithInvalidPrice()
        {
            var faker = new Faker();
            return new Product(
                title: faker.Commerce.ProductName(),
                price: 0,
                description: faker.Commerce.ProductDescription(),
                category: faker.Commerce.Categories(1)[0],
                image: faker.Image.PicsumUrl(),
                stockQuantity: faker.Random.Int(1, 100)
            );
        }

        /// <summary>
        /// Helper method to set private property values for testing.
        /// </summary>
        private static void SetPrivateProperty<T>(T instance, string propertyName, object value)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(instance, value);
            }
        }
    }
}