using Geography.Infrastructure.Application;
using OnlineShop.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geography.Services.Products.Contracts
{
    public interface ProductRepository : Repository
    {
        void Add(Product product);
        bool IsDuplicatedTitleInCategory(string title, int categoryId);
    }
}
