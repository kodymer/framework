using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vesta.Auditing;
using Vesta.Ddd.Domain.EventBus;
using Vesta.EntityFrameworkCore.Abstracts;
using Vesta.Uow;

namespace Vesta.EntityFrameworkCore
{
    public abstract class VestaDbContextBase<TDbContext> : DbContext, IEfCoreDbContext
        where TDbContext : DbContext
    {

        public IServiceProvider ServiceProvider { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public IUnitOfWorkEventRecordRegistrar UnitOfWorkEventRecordRegistrar { get; set; }

        protected VestaDbContextBase(DbContextOptions<TDbContext> options)
            : base(options)
        {
            AuditPropertySetter = NullAuditPropertySetter.Instance;
            UnitOfWorkEventRecordRegistrar = NullUnitOfWorkEventRecordRegistrar.Instance;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditConcepts();

            var eventReport = PrepareDispatchReport();

            var entriesWrittenToDatabase = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (entriesWrittenToDatabase > 0)
            {
                await RegisterDispatchReport(eventReport, cancellationToken);
            }

            return entriesWrittenToDatabase;
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

        private DispatchReport PrepareDispatchReport()
        {
            return UnitOfWorkEventRecordRegistrar.PrepareReport(ChangeTracker.Entries());
        }

        private async Task RegisterDispatchReport(DispatchReport dispatchEventReport, CancellationToken cancellationToken = default)
        {
            await UnitOfWorkEventRecordRegistrar.RegisterAsync(dispatchEventReport, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplySoftDeleteQueryFilterConcept();
        }

        protected abstract string GetConnectionString(DbContextOptions<TDbContext> options);
    }
}
