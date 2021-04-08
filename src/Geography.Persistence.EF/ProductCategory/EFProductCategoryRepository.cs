using Geography.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Entity;
using OnlineShop.Services.ProductCategories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Persistence.EF.ProductCategories
{
    public class EFProductCategoryRepository : ProductCategoryRepository
    {
        private readonly EFDataContext _context;

        public EFProductCategoryRepository(EFDataContext context)
        {
            _context = context;
        }

        public ProductCategory Add(ProductCategory category)
        {
            var newp=_context.ProductCategories.Add(category);
            return newp.Entity;
        }

        public async Task<ProductCategory> FindById(int categoryId)
        {
            return await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == categoryId);
        }

        public async Task<IList<ProductCategoryDto>> GetAll()
        {
            return await _context.ProductCategories.Select(_ => new ProductCategoryDto()
            {
                Id = _.Id,
                Title = _.Title
            }).ToListAsync();
        }

        public async Task<bool> IsDuplicatedInCategory(string title)
        {
            return await _context.ProductCategories.AnyAsync(_ => _.Title == title);
        }
    }
}
