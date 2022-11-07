using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Vesta.Auditing;
using Vesta.Ddd.Domain.EventBus;
using Vesta.EntityFrameworkCore.Abstracts;
using Vesta.Uow;

namespace Vesta.EntityFrameworkCore
{
    public abstract class VestaDbContextBase<TDbContext> : DbContext, IStartableEfCoreDbContext
        where TDbContext : DbContext
    {
        internal protected DbContextOptions Options { get; }

        public IServiceProvider ServiceProvider { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public IUnitOfWorkEventRecordRegistrar UnitOfWorkEventRecordRegistrar { get; set; }

        public VestaDbContextBase(DbContextOptions<TDbContext> options)
            : base(options)
        {
            Options = options;
            AuditPropertySetter = NullAuditPropertySetter.Instance;
            UnitOfWorkEventRecordRegistrar = NullUnitOfWorkEventRecordRegistrar.Instance;
        }

        void IStartableEfCoreDbContext.Initialize(EfCoreDbContextInitianlizationContext initializationContext)
        {
            if(initializationContext.UnitOfWork.Options.Timeout.HasValue && 
                Database.IsRelational()  && 
                !Database.GetCommandTimeout().HasValue)
            {
                Database.SetCommandTimeout(TimeSpan.FromMilliseconds(initializationContext.UnitOfWork.Options.Timeout.Value));
            }
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

        public virtual string GetConnectionString()
        {
            return string.Empty;
        }
    }
}
