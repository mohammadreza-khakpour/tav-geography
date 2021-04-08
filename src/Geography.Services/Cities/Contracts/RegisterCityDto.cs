using System.ComponentModel.DataAnnotations;

namespace Geography.Services.Cities.Contracts
{
    public class RegisterCityDto
    {
        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
