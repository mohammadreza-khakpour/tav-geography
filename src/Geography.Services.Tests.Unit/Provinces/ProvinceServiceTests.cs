using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Geography.Entities;
using Geography.Infrastructure.Application;
using Geography.Infrastructure.Test;
using Geography.Persistence.EF;
using Geography.Persistence.EF.Provinces;
using Geography.Services.Provinces;
using Geography.Services.Provinces.Contracts;
using Geography.Services.Provinces.Exceptions;
using Xunit;

namespace Geography.Services.Tests.Unit.Provinces
{
    public class ProvinceServiceTests
    {
        EFInMemoryDatabase database;
        EFDataContext dataContext;
        ProvinceRepository repository;
        UnitOfWork unitOfWork;
        ProvinceService sut;
        ProvinceService queryService;

        public ProvinceServiceTests()
        {
            database = new EFInMemoryDatabase();
            dataContext = database.CreateDataContext<EFDataContext>();
            repository = new EFProvinceRepository(dataContext);
            unitOfWork = new EFUnitOfWork(dataContext);
            sut = new ProvinceAppService(repository, unitOfWork);
            queryService = new ProvinceAppService(repository, unitOfWork);
        }

        [Fact]
        void Register_registers_a_Province_properly()
        {
            var dto = new RegisterProvinceDto
            {
                Name = "dummy"
            };
            sut.Register(dto);

            var provinces = queryService.GetAll();
            provinces.Should().HaveCount(1).And.Contain(_ => _.Name == "dummy");
        }

        [Fact]
        void Register_throws_exception_when_duplicate_name_passed()
        {
            dataContext.Manipulate(db =>
            {
                db.Provinces.Add(new Province { Name = "fars" });
            });

            var dto = new RegisterProvinceDto
            {
                Name = "fars"
            };
            Action action = () => sut.Register(dto);

            action.Should().Throw<DuplicateProvinceNameException>();
        }

        [Fact]
        void GetAll_retrieves_all_provinces_properly()
        {
            dataContext.Manipulate(db =>
            {
                db.Provinces.AddRange(new List<Province>
                {
                    new Province { Name = "tehran" },
                    new Province { Name = "fars" }
                });
            });
            var expectedResult = new[]
            {
                new { Name = "tehran" },
                new { Name = "fars" }
            };

            var actualResult = sut.GetAll();

            actualResult.Select(_ => new { _.Name }).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        void GetCities_retrieves_cities_of_a_province()
        {
            var province = new Province { Name = "dummy" };
            dataContext.Manipulate(db =>
            {
                db.Provinces.Add(province);
                db.Cities.Add(new City { Name = "tehran", Province = province });
                db.Cities.Add(new City { Name = "fars", Province = province });
            });

            var actualResult = sut.GetCities(province.Id);

            actualResult.Select(_ => new { _.Name }).Should().HaveCount(2).And.BeEquivalentTo(new[]
            {
                new { Name = "tehran" },
                new { Name = "fars" }
            });
        }
    }
}
