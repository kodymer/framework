using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EntityFrameworkCore.Modeling;
using Vesta.ProjectName.Bank;

namespace Vesta.ProjectName.EntityFrameworkCore
{
    public static class ProjectNameModelBuilderExtensions 
    {
        public static void ConfigureProjectName(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(b =>
            {
                b.ToTable("BankAccounts");
                b.HasKey(p => p.Id);

                b.ConfigureFullAuditedAggregateRoot();

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.Number).IsRequired().HasMaxLength(BankAccount.NameMaxLength);
                b.Property(p => p.Balance).IsRequired().HasColumnType("decimal").HasPrecision(16,4).HasDefaultValue(Decimal.Zero);
            });
        }
    }
}
