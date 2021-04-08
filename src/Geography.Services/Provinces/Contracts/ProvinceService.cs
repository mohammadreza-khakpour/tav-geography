using System;
using System.Collections.Generic;
using Geography.Infrastructure.Application;

namespace Geography.Services.Provinces.Contracts
{
    public interface ProvinceService : Service
    {
        void Register(RegisterProvinceDto dto);
        IList<GetAllProvincesDto> GetAll();
        IList<GetProvinceCitiesDto> GetCities(int id);
    }
}
