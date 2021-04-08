using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Geography.Services.Provinces.Contracts;

namespace Geography.RestApi.Controllers
{
    [ApiController, Route("api/provinces")]
    public class ProvinceController : ControllerBase
    {
        private readonly ProvinceService _service;

        public ProvinceController(ProvinceService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Register([FromBody] RegisterProvinceDto dto)
        {
            _service.Register(dto);
        }

        [HttpGet]
        public IList<GetAllProvincesDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}/cities")]
        public IList<GetProvinceCitiesDto> GetCities([FromRoute] int id)
        {
            return _service.GetCities(id);
        }
    }
}
