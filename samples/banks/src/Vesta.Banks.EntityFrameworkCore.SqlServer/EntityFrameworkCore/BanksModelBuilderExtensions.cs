using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EntityFrameworkCore.Modeling;
using Vesta.Banks.Bank;

namespace Vesta.Banks.EntityFrameworkCore
{
    public static class BanksModelBuilderExtensions 
    {
        public static void ConfigureBanks(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(b =>
            {
                b.ToTable("BankAccounts");
                b.HasKey(p => p.Id);

                b.ConfigureFullAuditedAggregateRoot();

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.Number).IsRequired().HasMaxLength(BankAccount.NameMaxLength);
                b.Property(p => p.Balance).IsRequired().HasColumnType("decimal").HasPrecision(16, 4).HasDefaultValue(Decimal.Zero);

                b.HasMany(p => p.Debits).WithOne().HasForeignKey(p => p.BankAccountFromId).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(p => p.Credits).WithOne().HasForeignKey(p => p.BankAccountToId).OnDelete(DeleteBehavior.Restrict);

            }).Entity<BankTransfer>(b => {

                b.ToTable("BankTransfers");
                b.HasKey(p => p.Id);

                b.ConfigureCreationAudited();

                b.Property(p => p.BankAccountFromId).IsRequired();
                b.Property(p => p.BankAccountToId).IsRequired();
                b.Property(p => p.Amount).IsRequired().HasColumnType("decimal").HasPrecision(16, 4).HasDefaultValue(Decimal.Zero);
            });
        }
    }
}
