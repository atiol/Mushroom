using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Helpers;
using Core.Entity.Abstracts;
using Core.Entity.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserRefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new RoleAclsEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var entityEntries = ChangeTracker.Entries();

            var now = DateTime.Now;

            entityEntries.ToList().ForEach(entry =>
            {
                if (!(entry.Entity is BaseEntity entity)) return;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedDate = now;
                        entity.CreatedBy = HttpContextHelper.GetUserName();
                        break;
                    case EntityState.Modified:
                        if (entity.DeletedDate == null)
                        {
                            entity.ModifiedDate = now;
                            entity.ModifiedBy = HttpContextHelper.GetUserName();
                        }
                        break;
                }
            });

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}