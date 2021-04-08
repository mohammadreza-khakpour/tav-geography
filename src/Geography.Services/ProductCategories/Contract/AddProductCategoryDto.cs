using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Services.ProductCategories.Contract
{
    public class AddProductCategoryDto
    {
        [Required]
        public string Title { get; set; }

        public AddProductCategoryDto(string title)
        {
            Title = title;
        }
    }
}
