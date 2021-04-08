using Geography.Infrastructure.Domain;

namespace Geography.Services.Cities.Exceptions
{
    public class CityNotFoundException : BusinessException
    {
        public int Id { get; set; }
    }
}
