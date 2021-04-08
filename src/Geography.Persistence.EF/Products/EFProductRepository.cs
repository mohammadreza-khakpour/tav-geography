using Geography.Services.Products.Contracts;
using OnlineShop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geography.Persistence.EF.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly EFDataContext _context;

        public EFProductRepository(EFDataContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public bool IsDuplicatedTitleInCategory(string title, int categoryId)
        {
            return _context.Products.Any(_ => _.Title == title && _.CategoryId == categoryId);
        }
    }
}
