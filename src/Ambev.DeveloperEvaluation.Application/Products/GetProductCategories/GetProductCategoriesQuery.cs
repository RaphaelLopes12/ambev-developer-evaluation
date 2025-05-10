using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategories;

/// <summary>
/// Query to get all product categories
/// </summary>
public class GetProductCategoriesQuery : IRequest<List<string>>
{
}
