using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Products.Commands;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public AddProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            Guid.NewGuid(),
            request.Name,
            request.Sku,
            request.Price
        );

        await _productRepository.AddAsync(product);

        return product.Id;
    }
}