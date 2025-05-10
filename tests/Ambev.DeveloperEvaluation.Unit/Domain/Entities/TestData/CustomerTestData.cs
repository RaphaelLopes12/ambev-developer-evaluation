using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Customer entity.
/// </summary>
public static class CustomerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Customer instances.
    /// </summary>
    private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
        .CustomInstantiator(f => new Customer(
            name: f.Name.FullName(),
            email: f.Internet.Email(),
            phone: f.Phone.PhoneNumber("###-###-####"),
            address: f.Address.FullAddress()
        ));

    /// <summary>
    /// Generates a valid Customer with random data.
    /// </summary>
    /// <returns>A valid Customer</returns>
    public static Customer GenerateValidCustomer()
    {
        return CustomerFaker.Generate();
    }

    /// <summary>
    /// Generates a Customer with a specific MongoDB ID.
    /// </summary>
    /// <param name="id">The MongoDB ID to use</param>
    /// <returns>A Customer with the specified ID</returns>
    public static Customer GenerateCustomerWithId(string id)
    {
        var customer = GenerateValidCustomer();
        // Use reflection to set the private ID property
        typeof(Customer).GetProperty("Id").SetValue(customer, id);
        return customer;
    }

    /// <summary>
    /// Generates a list of Customers with random data.
    /// </summary>
    /// <param name="count">The number of Customers to generate</param>
    /// <returns>A list of valid Customers</returns>
    public static List<Customer> GenerateValidCustomers(int count)
    {
        return CustomerFaker.Generate(count);
    }
}
