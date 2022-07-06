using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Vesta.Auditing;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.EntityFrameworkCore
{
    public abstract class VestaDbContextBase<TDbContext> : DbContext, IEfCoreDbContext
        where TDbContext : DbContext
    {

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        protected VestaDbContextBase(DbContextOptions<TDbContext> options)
            : base(options)
        {
            AuditPropertySetter = NullAuditPropertySetter.Instance;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditConcepts();
            
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // TODO: prepare for publish events
        }

        private void ApplyAuditConcepts()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ApplyAuditConceptsForAddedEntity(entry);
                        break;
                    case EntityState.Modified:
                        ApplyAuditConceptsForModifiedEntity(entry);
                        break;
                    case EntityState.Deleted:
                        ApplyAuditConceptsForDeletedEntity(entry);
                        break;
                }
            }
        }

        private void ApplyAuditConceptsForAddedEntity(EntityEntry entry)
        {
            AuditPropertySetter.SetCreationProperties(entry.Entity);
        }

        private void ApplyAuditConceptsForModifiedEntity(EntityEntry entry)
        {
            AuditPropertySetter.SetModificationProperties(entry.Entity);
        }

        private void ApplyAuditConceptsForDeletedEntity(EntityEntry entry)
        {
            AuditPropertySetter.SetDeletionProperties(entry.Entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplySoftDeleteQueryFilterConcept();
        }

        protected abstract string GetConnectionString(DbContextOptions<TDbContext> options);
    }
}
