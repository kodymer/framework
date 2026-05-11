using CompanyName.Auditing;
using CompanyName.Ddd.Domain.EventBus;
using CompanyName.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CompanyName.EntityFrameworkCore
{
    public abstract class CompanyNameDbContextBase<TContext> : DbContext, IInitializableDbContext
        where TContext : DbContext
    {
        internal protected DbContextOptions Options { get; }

        public IServiceProvider ServiceProvider { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public IUnitOfWorkEventRecordRegistrar UnitOfWorkEventRecordRegistrar { get; set; }

        protected CompanyNameDbContextBase(DbContextOptions<TContext> options)
            : base(options)
        {
            Options = options;
            AuditPropertySetter = NullAuditPropertySetter.Instance;
            UnitOfWorkEventRecordRegistrar = NullUnitOfWorkEventRecordRegistrar.Instance;
        }

        void IInitializableDbContext.Initialize(DbContextInitializationContext initializationContext)
        {
            if (initializationContext.UnitOfWork.Options.Timeout.HasValue &&
                Database.IsRelational() &&
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
