using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EntityFrameworkCore.Modeling;
using Vesta.Banks.Bank;
using System.Reflection.Emit;
using Vesta.Banks.Traceability;

namespace Vesta.Banks.EntityFrameworkCore
{
    public static class BanksModelBuilderExtensions 
    {
        public static void ConfigureBanks(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(b =>
            {
                b.ToTable(BankAccount.TableName);
                b.HasKey(p => p.Id);

                b.ConfigureFullAuditedAggregateRoot();

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.Number).IsRequired().HasMaxLength(BankAccount.NameMaxLength);
                b.Property(p => p.Balance).IsRequired().HasColumnType("decimal").HasPrecision(16, 4);

            }).Entity<BankTransfer>(b => {

                b.ToTable(BankTransfer.TableName);
                b.HasKey(p => p.Id);

                b.ConfigureCreationAudited();

                b.Property(p => p.BankAccountFromNumber).IsRequired();
                b.Property(p => p.BankAccountToNumber).IsRequired();
                b.Property(p => p.Amount).IsRequired().HasColumnType("decimal").HasPrecision(16, 4);
            });
        }
        public static void ConfigureTraceability(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Error>(b =>
            {
                b.ToTable(Error.TableName);
                b.HasKey(p => p.Id);

                b.ConfigureCreationAuditedAggregateRoot();

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.Type).IsRequired().HasMaxLength(Error.TypeMaxLength);
                b.Property(p => p.Message).IsRequired().HasMaxLength(Error.MessageMaxLength);
                b.Property(p => p.StackTrace).IsRequired().HasMaxLength(Error.StackTraceMaxLength);

            });
        }
    }
}
