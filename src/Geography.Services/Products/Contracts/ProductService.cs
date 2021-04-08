using Geography.Infrastructure.Application;
using Geography.Services.Tests.Unit.Products.Add;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geography.Services.Products.Contracts
{
    public interface ProductService : Service
    {
        int Add(AddProductDto dto);
    }
}
