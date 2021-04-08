using Geography.Infrastructure.Domain;

namespace Geography.Entities
{
    public class City : Entity<int>
    {
        public string Name { get; set; }
        public Province Province { get; set; }
        public int ProvinceId { get; set; }
    }
}
