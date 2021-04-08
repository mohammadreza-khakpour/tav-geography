using FluentAssertions;
using Geography.Infrastructure.Test;
using Geography.Persistence.EF;
using Geography.Persistence.EF.Products;
using Geography.Services.Products.Contracts;
using Geography.Services.Products.Exceptions;
using OnlineShop.Entity;
using System;
using Xunit;

namespace Geography.Services.Tests.Unit.Products.Add
{
    public class FailedWhendDuplicatedTitle
    {
        private readonly EFDataContext _context;
        private readonly ProductService _sut;
        private Product _product;
        private Action _expected;

        public FailedWhendDuplicatedTitle()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            var unitOfWork = new EFUnitOfWork(_context);
            var repository = new EFProductRepository(_context);
            _sut = new ProductAppService(repository, unitOfWork);
        }

        // فرض میکنم یک دسته بندی با عنوان لبنیات در فهرست دسته بندی کالا وجود دارد و کالایی
        // با نام ماست و کد 02 و حداقل موجودی 10 در فهرست کالاها و دسته لبنیات وجود دارد
        private void Given()
        {
            var productCategory = new ProductCategory
            {
                Title = "لبنیات"
            };
            _product = new Product
            {
                Title = "ماست",
                Code = "02",
                MinimumInventory = 10,
                ProductCategory = productCategory
                //CategoryId = productCategory.Id
            };
            _context.Manipulate(_ => _.Products.Add(_product));
        }

        // زمانی که یک کالا با نام ماست و کد 03 و حداقل موجودی 20 در فهرست کالاهاودسته
        // لبنیات تعریف میکنم
        private void When()
        {
            var dto = new AddProductDto
            {
                Title = _product.Title,
                Code = "03",
                MinimumInventory = 20,
                CategoryId = _product.CategoryId
            };
           _expected = () => _sut.Add(dto);
        }

        // بنابرین باید خطای تکراری بودن ماست در دسته لبنیات نشان داده شود
        // و باید فقط یک کالا با عنوان ماست و کد 02 و حداقل موجودی 10 در
        // دسته لبنیات موجود باشد
        private void Then()
        {
            _expected.Should()
                .ThrowExactly<FailedWhenDuplicatedTitleException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
