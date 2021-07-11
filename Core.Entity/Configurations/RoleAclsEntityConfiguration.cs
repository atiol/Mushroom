using Core.Entity.Abstracts;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configurations
{
    public sealed class RoleAclsEntityConfiguration : BaseEntityConfiguration<RoleAclsEntity>
    {
        public override void Configure(EntityTypeBuilder<RoleAclsEntity> builder)
        {
            builder.ToTable("RoleAcls");
            
            builder.Property(p => p.AclKey).IsRequired();
            
            builder.HasOne(roleAcl => roleAcl.Role)
                .WithMany(role => role.RoleAcls)
                .HasForeignKey(roleAcl => roleAcl.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            base.Configure(builder);
        }
    }
}