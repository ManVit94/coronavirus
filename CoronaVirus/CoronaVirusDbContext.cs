using CoronaVirus.Domain;
using CoronaVirus.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirus
{
    public class CoronaVirusDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<VirusStatistic> VirusStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new VirusStatisticConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=WS-LV-CP2854\\SQLEXPRESS;Database=CoronaVirusDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
