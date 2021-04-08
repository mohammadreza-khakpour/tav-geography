using Geography.Infrastructure.Application;

namespace Geography.Services.Cities.Contracts
{
    public interface CityService : Service
    {
        void Register(RegisterCityDto dto);
        FindCityByIdDto? FindById(int id);
    }
}
