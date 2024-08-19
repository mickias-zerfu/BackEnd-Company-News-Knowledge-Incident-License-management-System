using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class TransactionMonitoringContext : DbContext
    {
        public TransactionMonitoringContext(DbContextOptions<TransactionMonitoringContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your entity mappings here, if necessary
            // e.g., modelBuilder.Entity<Transaction>().ToTable("TRANSACTIONS");
        }
    }
}
