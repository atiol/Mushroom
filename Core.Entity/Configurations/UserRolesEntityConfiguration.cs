using Core.Entity.Abstracts;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configurations
{
    public sealed class UserRolesEntityConfiguration : BaseEntityConfiguration<UserRolesEntity>
    {
        public override void Configure(EntityTypeBuilder<UserRolesEntity> builder)
        {
            builder.ToTable("UserRoles");
            
            builder.HasOne(userRole => userRole.Role)
                .WithMany(role => role.UserRoles)
                .HasForeignKey(userRole => userRole.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(userRole => userRole.User)
                .WithMany(user => user.UserRoles)
                .HasForeignKey(userRole => userRole.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            base.Configure(builder);
        }
    }
}