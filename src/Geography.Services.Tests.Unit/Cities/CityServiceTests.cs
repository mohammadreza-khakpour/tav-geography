using System.Collections.Generic;
using FluentAssertions;
using Geography.Entities;
using Geography.Infrastructure.Application;
using Geography.Infrastructure.Test;
using Geography.Persistence.EF;
using Geography.Persistence.EF.Cities;
using Geography.Persistence.EF.Provinces;
using Geography.Services.Cities;
using Geography.Services.Cities.Contracts;
using Geography.Services.Provinces;
using Geography.Services.Provinces.Contracts;
using Xunit;

namespace Geography.Services.Tests.Unit.Cities
{
    public class CityServiceTests
    {
        EFInMemoryDatabase database;
        EFDataContext dataContext;
        CityRepository repository;
        UnitOfWork unitOfWork;
        CityService sut;
        ProvinceService queryService;

        public CityServiceTests()
        {
            database = new EFInMemoryDatabase();
            dataContext = database.CreateDataContext<EFDataContext>();
            repository = new EFCityRepository(dataContext);
            unitOfWork = new EFUnitOfWork(dataContext);
            sut = new CityAppService(repository, unitOfWork);
            var queryRepository = new EFProvinceRepository(dataContext);
            queryService = new ProvinceAppService(queryRepository, unitOfWork);
        }

        [Fact]
        void Register_registers_a_City_properly()
        {
            var province = new Province { Name = "dummy" };
            dataContext.Manipulate(db =>
            {
                db.Provinces.Add(province);
            });

            var dto = new RegisterCityDto
            {
                Name = "dummy",
                ProvinceId = province.Id
            };
            sut.Register(dto);

            var cities = queryService.GetCities(province.Id);
            cities.Should().HaveCount(1).And.Contain(_ => _.Name == "dummy");
        }
        
        [Fact]
        void FindById_finds_city_by_id_properly()
        {
            var city = new City { Name = "shiraz" };
            dataContext.Manipulate(db =>
            {
                var province = new Province
                {
                    Name = "fars",
                    Cities = new HashSet<City>(new[] { city })
                };
                db.Provinces.Add(province);
            });

            var actualResult = sut.FindById(city.Id);

            actualResult.Should().BeEquivalentTo(new
            {
                city.Id,
                city.Name,
                city.ProvinceId
            });
        }
    }
}
