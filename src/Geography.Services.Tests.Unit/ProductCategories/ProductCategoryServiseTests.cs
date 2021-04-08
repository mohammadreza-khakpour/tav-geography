using FluentAssertions;
using Geography.Infrastructure.Application;
using Geography.Infrastructure.Test;
using Geography.Persistence.EF;
using OnlineShop.Entity;
using OnlineShop.Persistence.EF.ProductCategories;
using OnlineShop.Services.ProductCategories;
using OnlineShop.Services.ProductCategories.Contract;
using OnlineShop.Services.ProductCategories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geography.Services.Tests.Unit.ProductCategories
{
    public class ProductCategoryServiseTests
    {
        private UnitOfWork unitOfWork;
        private ProductCategoryRepository repository;
        private ProductCategoryService sut;
        private EFDataContext context;
        private EFDataContext readDataContext;

        public ProductCategoryServiseTests()
        {
            var db = new EFInMemoryDatabase();
            context = db.CreateDataContext<EFDataContext>();
            readDataContext = db.CreateDataContext<EFDataContext>();
            repository = new EFProductCategoryRepository(context);
            unitOfWork = new EFUnitOfWork(context);
            sut = new ProductCategoryAppService(repository, unitOfWork);
        }
        [Fact]
        public async void Add_add_productCategory_properly()
        {
            var dto = ProductCategoryFactory.GenerateADDDto();

            var actual = await sut.Add(dto);

            var expected = readDataContext.ProductCategories.Single(_ => _.Id == actual);
            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Add_prevent_add_when_title_exists()
        {
            var productCategory = new ProductCategory()
            {
                Title = "dummy-title"
            };
            context.Manipulate(_ => _.ProductCategories.Add(productCategory));
            var dto = ProductCategoryFactory.GenerateADDDto(productCategory.Title);


            Func<Task> expected = () => sut.Add(dto);

            expected.Should().Throw<IsDuplicatedProductCategoryException>();
        }
    }
}
