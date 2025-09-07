using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;