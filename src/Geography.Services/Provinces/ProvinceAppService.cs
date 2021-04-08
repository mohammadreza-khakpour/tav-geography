using System.Collections.Generic;
using Geography.Entities;
using Geography.Infrastructure.Application;
using Geography.Services.Provinces.Contracts;
using Geography.Services.Provinces.Exceptions;

namespace Geography.Services.Provinces
{
    public class ProvinceAppService : ProvinceService
    {
        private readonly ProvinceRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProvinceAppService(ProvinceRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Register(RegisterProvinceDto dto)
        {
            if (_repository.ExistsByName(dto.Name))
            {
                throw new DuplicateProvinceNameException { Name = dto.Name };
            }

            var province = new Province
            {
                Name = dto.Name
            };

            _repository.Add(province);

            _unitOfWork.Complete();
        }

        public IList<GetAllProvincesDto> GetAll()
        {
            return _repository.GetAllProvinces();
        }

        public IList<GetProvinceCitiesDto> GetCities(int id)
        {
            return _repository.GetCities(id);
        }
    }
}
