using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    decimal Price
);