using System.Collections.Generic;
using Geography.Entities;
using Geography.Infrastructure.Application;

namespace Geography.Services.Provinces.Contracts
{
    public interface ProvinceRepository : Repository
    {
        void Add(Province province);
        bool ExistsByName(string name);
        IList<GetAllProvincesDto> GetAllProvinces();
        IList<GetProvinceCitiesDto> GetCities(int id);
    }
}
