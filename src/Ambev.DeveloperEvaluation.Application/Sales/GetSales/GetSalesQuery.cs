using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    /// <summary>
    /// Query to retrieve a paginated list of sales.
    /// </summary>
    public class GetSalesQuery : IRequest<GetSalesResult>
    {
        /// <summary>
        /// Page number (1-based).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
