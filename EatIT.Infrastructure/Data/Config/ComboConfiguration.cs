using EatIT.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIT.Infrastructure.Data.Config
{
    public class ComboConfiguration : IEntityTypeConfiguration<Combo>
    {
        public void Configure(EntityTypeBuilder<Combo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ComboDescription)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.HasOne(x => x.Dish)
                .WithMany(d => d.Combos)
                .HasForeignKey(x => x.DishId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
