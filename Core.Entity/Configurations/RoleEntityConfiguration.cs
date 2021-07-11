using Core.Entity.Abstracts;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configurations
{
    public sealed class RoleEntityConfiguration : BaseEntityConfiguration<RoleEntity>
    {
        public override void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Roles");
            
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.NormalizedName).IsRequired();
            
            base.Configure(builder);
        }
    }
}