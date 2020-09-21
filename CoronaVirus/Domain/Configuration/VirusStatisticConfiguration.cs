using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirus.Domain.Configuration
{
    public class VirusStatisticConfiguration : IEntityTypeConfiguration<VirusStatistic>
    {
        public void Configure(EntityTypeBuilder<VirusStatistic> builder)
        {
            builder.HasKey(vs => vs.Id);

            builder.Property(vs => vs.ReportDate)
                .HasColumnType("date");

            builder.HasOne(vs => vs.Country)
                .WithMany(c => c.VirusStatistics)
                .HasForeignKey(vs => vs.CountryId);

            builder.HasIndex(vs => new { vs.ReportDate, vs.CountryId }).IsUnique();
        }
    }
}
