using System.ComponentModel.DataAnnotations;

namespace Geography.Services.Provinces.Contracts
{
    public class RegisterProvinceDto
    {
        [Required]
        public string Name { get; set; }
    }
}
