using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Configurations
{
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members");

            builder.HasOne<Member>()
                   .WithOne(X => X.HealthRecord)
                   .HasForeignKey<HealthRecord>(X => X.Id);

            builder.Property(X=>X.UpdatedAt)
                .HasColumnName("LastUpdated")
                .HasDefaultValueSql("GETDATE()");

            builder.Ignore(X => X.Id);
            builder.Ignore(X => X.CreatedAt);
        }
    }
}
