using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for retrieving a paginated list of sales.
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler.
    /// </summary>
    /// <param name="saleRepository">Sale repository</param>
    /// <param name="mapper">Automapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to retrieve a paginated list of sales.
    /// </summary>
    /// <param name="request">The query parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of sales</returns>
    public async Task<GetSalesResult> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (sales, totalCount) = await _saleRepository.GetPagedAsync(page, pageSize, cancellationToken);

        var result = new GetSalesResult
        {
            Items = _mapper.Map<List<GetSaleListItemResult>>(sales),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        foreach (var saleListItem in result.Items)
        {
            var sale = sales.FirstOrDefault(s => s.Id == saleListItem.Id);
            if (sale != null)
            {
                saleListItem.ItemCount = sale.Items.Count;
            }
        }

        return result;
    }
}