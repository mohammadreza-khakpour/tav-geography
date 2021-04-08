using Geography.Entities;
using Geography.Infrastructure.Application;
using Geography.Services.Cities.Contracts;

namespace Geography.Services.Cities
{
    public class CityAppService : CityService
    {
        private readonly CityRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CityAppService(CityRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Register(RegisterCityDto dto)
        {
            var city = new City
            {
                Name = dto.Name,
                ProvinceId = dto.ProvinceId
            };
            _repository.Add(city);
            _unitOfWork.Complete();
        }

        public FindCityByIdDto FindById(int id)
        {
            return _repository.FindById(id);
        }
    }
}
