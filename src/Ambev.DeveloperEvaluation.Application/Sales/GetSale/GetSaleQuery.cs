using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Query to retrieve a sale by ID.
    /// </summary>
    public class GetSaleQuery : IRequest<GetSaleResult>
    {
        /// <summary>
        /// The unique identifier of the sale to retrieve.
        /// </summary>
        public Guid Id { get; set; }
    }
}
