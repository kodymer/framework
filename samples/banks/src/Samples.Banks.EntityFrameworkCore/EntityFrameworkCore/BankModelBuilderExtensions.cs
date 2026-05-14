using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Samples.Banks.Accounts;
using Samples.Banks.Traceability;
using Samples.Banks.Transfers;

namespace Samples.Banks.EntityFrameworkCore
{
    public static class BankModelBuilderExtensions
    {
        public static void ConfigureBanks(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(b =>
            {
                b.ToTable(BankAccount.TableName);
                b.HasKey(p => p.Id);

                b.ConfigureFullAuditedAggregateRoot();

                b.Property(p => p.Id).IsRequired().ValueGeneratedNever().HasConversion<EntityIdToKeyConverter<BankAccountId, Guid>>();
                b.Property(p => p.Number).IsRequired().HasMaxLength(BankAccount.NameMaxLength);
                b.Property(p => p.Balance).IsRequired().HasColumnType("decimal").HasPrecision(16, 4);

            }).Entity<BankTransfer>(b =>
            {

                b.ToTable(BankTransfer.TableName);
                b.HasKey(p => p.Id);

                b.ConfigureCreationAudited();

                b.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd().HasConversion<EntityIdToKeyConverter<BankTransferId, long>>();
                b.Property(p => p.BankAccountFromNumber).IsRequired();
                b.Property(p => p.BankAccountToNumber).IsRequired();
                b.Property(p => p.Amount).IsRequired().HasColumnType("decimal").HasPrecision(16, 4);
            });
        }
        public static void ConfigureTraceability(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ErrorRecord>(b =>
            {
                b.ToTable(ErrorRecord.TableName);
                b.HasKey(p => p.Id);

                b.ConfigureCreationAuditedAggregateRoot();

                b.Property(p => p.Id).IsRequired().ValueGeneratedNever().HasConversion<EntityIdToKeyConverter<ErrorRecordId, Guid>>();
                b.Property(p => p.Type).IsRequired().HasMaxLength(ErrorRecord.TypeMaxLength);
                b.Property(p => p.Message).IsRequired().HasMaxLength(ErrorRecord.MessageMaxLength);
                b.Property(p => p.StackTrace).IsRequired().HasMaxLength(ErrorRecord.StackTraceMaxLength);

            });
        }
    }

    public class EntityIdToKeyConverter<TEntityId, TKey> : ValueConverter<TEntityId, TKey>
        where TEntityId : class, IEntityId<TKey>
    {
        public EntityIdToKeyConverter()
            : base(
                  entityId => entityId.Value,
                  key => (TEntityId)Activator.CreateInstance(typeof(TEntityId), key))
        {
        }
    }
}
