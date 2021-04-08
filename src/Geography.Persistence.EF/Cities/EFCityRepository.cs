using System.Linq;
using Microsoft.EntityFrameworkCore;
using Geography.Entities;
using Geography.Services.Cities.Contracts;

namespace Geography.Persistence.EF.Cities
{
    public class EFCityRepository : CityRepository
    {
        private readonly EFDataContext _dataContext;
        private readonly DbSet<City> _set;

        public EFCityRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
            _set = _dataContext.Cities;
        }

        public void Add(City city)
        {
            _set.Add(city);
        }

        public FindCityByIdDto? FindById(int id)
        {
            return _set.Where(_ => _.Id == id).Select(_ => new FindCityByIdDto
            {
                Id = _.Id,
                Name = _.Name,
                ProvinceId = _.ProvinceId

            }).FirstOrDefault();
        }
    }
}
