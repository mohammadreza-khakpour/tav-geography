using Geography.Infrastructure.Application;
using Geography.Services.Products.Contracts;
using Geography.Services.Products.Exceptions;
using OnlineShop.Entity;
using System;

namespace Geography.Services.Tests.Unit.Products.Add
{
    public class ProductAppService : ProductService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
        public ProductAppService(ProductRepository repository ,UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public int Add(AddProductDto dto)
        {
            if (_repository.IsDuplicatedTitleInCategory(dto.Title, dto.CategoryId))
            {
                throw new FailedWhenDuplicatedTitleException();
            }

            var product = new Product()
            {
                CategoryId = dto.CategoryId,
                Code = dto.Code,
                MinimumInventory = dto.MinimumInventory,
                Title = dto.Title,
            };

            _repository.Add(product);
             _unitOfWork.Complete();

            return product.Id;
        }
    }
}
