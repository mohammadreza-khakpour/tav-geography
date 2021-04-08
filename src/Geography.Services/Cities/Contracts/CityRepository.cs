using System.Collections.Generic;
using Geography.Entities;
using Geography.Infrastructure.Application;

namespace Geography.Services.Cities.Contracts
{
    public interface CityRepository : Repository
    {
        void Add(City city);
        FindCityByIdDto? FindById(int id);
    }
}
