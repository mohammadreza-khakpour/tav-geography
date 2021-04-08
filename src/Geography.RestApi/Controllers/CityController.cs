using Microsoft.AspNetCore.Mvc;
using Geography.Services.Cities.Contracts;

namespace Geography.RestApi.Controllers
{
    [ApiController, Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly CityService _service;

        public CityController(CityService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Register([FromBody] RegisterCityDto dto)
        {
            _service.Register(dto);
        }

        [HttpGet("{id}")]
        public FindCityByIdDto? FindById([FromRoute] int id)
        {
            return _service.FindById(id);
        }
    }
}
