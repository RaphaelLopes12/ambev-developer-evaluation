using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    /// <summary>
    /// Handler for deleting a product
    /// </summary>
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteProductHandler
        /// </summary>
        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Handles the delete product command
        /// </summary>
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            return await _productRepository.RemoveAsync(request.Id);
        }
    }
}
