using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Geography.Entities;

namespace Geography.Persistence.EF.Cities
{
    class CityEntityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> _)
        {
            _.ToTable("Cities");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).IsRequired().ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired().IsUnicode().HasMaxLength(100);
            _.Property(_ => _.ProvinceId);
        }
    }
}
