using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Customers.Queries;

public class LoginCustomerQueryHandler : IRequestHandler<LoginCustomerQuery, string?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCustomerQueryHandler(ICustomerRepository customerRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string?> Handle(LoginCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(request.Email);
        if (customer is null)
        {
            return null;
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        if (customer.HashedPassword != hashedPassword)
        {
            return null;
        }

        var token = _jwtProvider.GenerateToken(customer);

        return token;
    }
}