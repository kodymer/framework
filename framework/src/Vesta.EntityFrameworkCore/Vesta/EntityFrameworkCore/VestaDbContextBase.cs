using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;
using Vesta.Auditing;

namespace Vesta.EntityFrameworkCore
{
    public abstract class VestaDbContextBase<TDbContext> : DbContext
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

            // TODO: publish events
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

        public void ApplyAuditConceptsForAddedEntity(EntityEntry entry)
        {
            AuditPropertySetter.SetCreationProperties(entry.Entity);
        }

        public void ApplyAuditConceptsForModifiedEntity(EntityEntry entry)
        {
            AuditPropertySetter.SetModificationProperties(entry.Entity);
        }

        public void ApplyAuditConceptsForDeletedEntity(EntityEntry entry)
        {
            AuditPropertySetter.SetDeletionProperties(entry.Entity);
        }

    }
}
