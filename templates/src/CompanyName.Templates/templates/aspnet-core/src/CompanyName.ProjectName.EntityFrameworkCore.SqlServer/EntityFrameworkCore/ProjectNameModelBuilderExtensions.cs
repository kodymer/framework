using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EntityFrameworkCore.Modeling;

namespace Vesta.ProjectName.EntityFrameworkCore
{
    public static class ProjectNameModelBuilderExtensions 
    {
        public static void ConfigureProjectName(this ModelBuilder modelBuilder)
        {
            /*
            *   Samples...
            *
            *   modelBuilder.Entity<MyAggregateRoot>(b =>
            *   {
            *       b.ToTable("MyTableName");
            *       b.HasKey(p => p.Id);
            *       b.ConfigureFullAuditedAggregateRoot();
            *   });
            *
            *
            */
        }
    }
}
