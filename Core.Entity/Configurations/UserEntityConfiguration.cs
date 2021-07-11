using Core.Entity.Abstracts;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configurations
{
    public sealed class UserEntityConfiguration : BaseEntityConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            
            builder.Property(p => p.UserName).IsRequired();
            builder.Property(p => p.NormalizedUserName).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.NormalizedEmail).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.SurName).IsRequired();
            builder.Property(p => p.Password).IsRequired();
            
            base.Configure(builder);
        }
    }
}