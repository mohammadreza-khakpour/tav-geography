using FluentAssertions;
using Geography.Infrastructure.Test;
using Geography.Persistence.EF;
using Geography.Persistence.EF.Products;
using Geography.Services.Products.Contracts;
using OnlineShop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Geography.Services.Tests.Unit.Products.Add
{
    // [As a]
    public class Succesfull
    {
        ProductCategory productCategory;
        EFDataContext context;
        int productId;
        public Succesfull()
        {
            var db = new EFInMemoryDatabase();
            context = db.CreateDataContext<EFDataContext>();
        }
        //[Given("یک دسته بندی به نام لبنیات در فهرست دسته بندی کالاها وجود دارد")]
        private void Given()
        {
            productCategory = new ProductCategory { Title = "لبنیات" };
            context.Manipulate(_ => _.ProductCategories.Add(productCategory));
        }

        //[When("کالایی با نام ماست و کد 1006 و حداقل موجودی 10 را در دسته بندی لبنیات، تعریف میکنم")]
        private void When()
        {
            var repository = new EFProductRepository(context);
            var unitOfWork = new EFUnitOfWork(context);
            var sut = new ProductAppService(repository, unitOfWork);
            var dto = new AddProductDto()
            {
                CategoryId = productCategory.Id,
                Code = "1006",
                MinimumInventory = 10,
                Title = "ماست"
            };
            productId = sut.Add(dto);
        }

        //[Then("باید یک کالا با نام ماست و کد 2و حداقل موجودی 10
        //در فهرست کالاهای با دسته بندی لبنیات وجود داشته باشد")]
        private void Then()
        {
            var expectd = context.Products.Single(_ => _.Id == productId);
            expectd.Title.Should().Be("ماست");
            expectd.CategoryId.Should().Be(productCategory.Id);
            expectd.Code.Should().Be("1006");
            expectd.MinimumInventory.Should().Be(10);
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
