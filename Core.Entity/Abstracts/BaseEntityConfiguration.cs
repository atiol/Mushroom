using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Abstracts
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasQueryFilter(p => p.DeletedDate == null);
            
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.ModifiedBy);
            builder.Property(p => p.DeletedBy);

            builder.HasIndex(i => i.DeletedDate);
        }
    }
}