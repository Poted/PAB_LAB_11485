using MediatR;

namespace Application.Customers.Queries;

public record LoginCustomerQuery(
    string Email,
    string Password) : IRequest<string?>;