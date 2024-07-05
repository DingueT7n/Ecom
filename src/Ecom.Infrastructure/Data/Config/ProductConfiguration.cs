using Ecom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(30);
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)");
            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            builder.HasData(

                new Product { Id = 1, Name = " Product One", Description = "P1", Price = 11.5m, CategoryId = 1 },
                new Product { Id = 2, Name = " Product Two", Description = "P2", Price = 10.5m, CategoryId = 2 },
                new Product { Id = 3, Name = " Product Three", Description = "P3", Price = 0m, CategoryId = 3 }

                );

        }
    }
}
