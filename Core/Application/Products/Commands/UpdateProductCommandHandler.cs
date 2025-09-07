using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productToUpdate = await _productRepository.GetByIdAsync(request.Id);

        if (productToUpdate is null)
        {
            throw new Exception("Product not found.");
        }

        productToUpdate.UpdateDetails(request.Name, request.Sku, request.Price);

        await _productRepository.UpdateAsync(productToUpdate);
    }
}