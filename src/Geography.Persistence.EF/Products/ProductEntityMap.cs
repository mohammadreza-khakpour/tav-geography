using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Persistence.EF.Products
{
    public class ProductEntityMap : IEntityTypeConfiguration<Entity.Product>
    {
        public void Configure(EntityTypeBuilder<Entity.Product> _)
        {
            _.ToTable("Products");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Title).IsRequired(true).HasMaxLength(150);
            _.Property(_ => _.Code).IsRequired(true);
            _.Property(_ => _.MinimumInventory).IsRequired(true);
            _.Property(_ => _.CategoryId).IsRequired(true);

            _.HasOne(_ => _.ProductCategory).WithMany(_ => _.Products)
                .HasForeignKey(_ => _.CategoryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
