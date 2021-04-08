using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.ProductCategories.Contract
{
    public interface ProductCategoryService
    {
        Task<IList<ProductCategoryDto>> GetAll();
        Task<int> Add(AddProductCategoryDto dto);
        Task<int> Update(int id,AddProductCategoryDto dto);
    }
}
