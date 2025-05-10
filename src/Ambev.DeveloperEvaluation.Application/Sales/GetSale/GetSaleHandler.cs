using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Handler for retrieving a sale by ID.
    /// </summary>
    public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetSaleHandler.
        /// </summary>
        /// <param name="saleRepository">Sale repository</param>
        /// <param name="mapper">Automapper instance</param>
        public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve a sale by ID.
        /// </summary>
        /// <param name="request">The query parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details or null if not found</returns>
        public async Task<GetSaleResult> Handle(GetSaleQuery request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                return null;

            return _mapper.Map<GetSaleResult>(sale);
        }
    }
}
