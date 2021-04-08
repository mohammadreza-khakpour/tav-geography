using System.Collections.Generic;
using Geography.Infrastructure.Domain;

namespace Geography.Entities
{
    public class Province : Entity<int>
    {
        public Province()
        {
            Cities = new HashSet<City>();
        }
        public string Name { get; set; }
        public HashSet<City> Cities { get; set; }
    }
}
