using Application.Products.Dtos;
using Domain.Interfaces;
using MediatR;

namespace Application.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return null;
        }

        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Sku,
            product.Price
        );

        return productDto;
    }
}