using OnlineShop.Services.ProductCategories.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geography.Services.Tests.Unit.ProductCategories
{
    public static class ProductCategoryFactory
    {
        public static AddProductCategoryDto GenerateADDDto(string title = "dummy-title")
        {
            return new AddProductCategoryDto(title);
        }
    }
}
