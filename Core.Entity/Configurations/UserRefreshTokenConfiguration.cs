using Core.Entity.Abstracts;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configurations
{
    public sealed class UserRefreshTokenConfiguration : BaseEntityConfiguration<UserRefreshTokenEntity>
    {
        public override void Configure(EntityTypeBuilder<UserRefreshTokenEntity> builder)
        {
            builder.ToTable("UserRefreshTokens");
            
            builder.Property(p => p.Token).IsRequired();

            builder.HasOne(userT => userT.User)
                .WithMany(user => user.UserRefreshTokens)
                .HasForeignKey(userT => userT.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            base.Configure(builder);
        }
    }
}