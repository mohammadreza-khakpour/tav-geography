using Geography.Infrastructure.Domain;

namespace Geography.Services.Provinces.Exceptions
{
    public class DuplicateProvinceNameException : BusinessException
    {
        public string Name { get; set; }
    }
}
