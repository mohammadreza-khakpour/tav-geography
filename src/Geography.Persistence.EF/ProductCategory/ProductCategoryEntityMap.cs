using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Persistence.EF.ProductCategories
{
    public class ProductCategoryEntityMap : IEntityTypeConfiguration<Entity.ProductCategory>
    {
        public void Configure(EntityTypeBuilder<Entity.ProductCategory> _)
        {
            _.ToTable("ProductCategories");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Title).IsRequired(true).HasMaxLength(50);
        }
    }
}
