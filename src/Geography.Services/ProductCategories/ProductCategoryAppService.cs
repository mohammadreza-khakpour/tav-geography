using Geography.Infrastructure.Application;
using OnlineShop.Entity;

using OnlineShop.Services.ProductCategories.Contract;
using OnlineShop.Services.ProductCategories.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.ProductCategories
{
    public class ProductCategoryAppService : ProductCategoryService
    {
        private readonly ProductCategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProductCategoryAppService(ProductCategoryRepository repository, UnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AddProductCategoryDto dto)
        {
            if (await _repository.IsDuplicatedInCategory(dto.Title) == true)
            {
                throw new IsDuplicatedProductCategoryException();
            }

            ProductCategory productCategory = new ProductCategory()
            {
                Title = dto.Title,
            };

            _repository.Add(productCategory);
             _unitOfWork.Complete();

            return productCategory.Id;
        }

        public Task<IList<ProductCategoryDto>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<int> Update(int id, AddProductCategoryDto dto)
        {
            ProductCategory result = await _repository.FindById(id);
            if (result == null)
            {
                throw new NotFoundException();
            }
            result.Title = dto.Title;

           _unitOfWork.Complete();
            return result.Id;
        }
    }
}
