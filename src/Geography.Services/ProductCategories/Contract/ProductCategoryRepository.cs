using OnlineShop.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.ProductCategories.Contract
{
    public interface ProductCategoryRepository
    {
        Task<IList<ProductCategoryDto>> GetAll();
        Task<ProductCategory> FindById(int categoryId);
        ProductCategory Add(ProductCategory category);
        Task<bool> IsDuplicatedInCategory(string title);
    }
}
