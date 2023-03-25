using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Assignment>()
                .Property(e => e.Status)
                .HasDefaultValue(Domain.Enums.Status.Pending);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.Importance)
                .HasDefaultValue(Domain.Enums.Importance.Low);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.DueDate)
                .HasDefaultValue(DateTime.Now.AddDays(7));
        }

        public DbSet<Assignment> Assignments => Set<Assignment>();
    }
}