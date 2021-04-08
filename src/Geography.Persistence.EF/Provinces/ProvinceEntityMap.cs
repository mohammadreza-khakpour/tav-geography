using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Geography.Entities;

namespace Geography.Persistence.EF.Provinces
{
    class ProvinceEntityMap : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> _)
        {
            _.ToTable("Provinces");

            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).IsRequired().ValueGeneratedOnAdd();

            _.Property(_ => _.Name).IsRequired().HasMaxLength(100).IsUnicode();

            _.HasMany(_ => _.Cities).WithOne(_ => _.Province)
                .HasForeignKey(_ => _.ProvinceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
