using CNNCStorageDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB.Configurations
{
    public class WorkerConfig : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.Property(w => w.FirstName).IsRequired()
                                              .HasMaxLength(50);
            builder.Property(w => w.MiddleName).IsRequired()
                                             .HasMaxLength(100);
            builder.Property(w => w.LastName).IsRequired()
                                             .HasMaxLength(100);
            builder.Property(w => w.Email).IsRequired()
                                          .HasMaxLength(100);
            builder.Property(w => w.Phone).IsRequired()
                                          .HasMaxLength(13);
            builder.Property(w => w.Birthdate).IsRequired();
            builder.Property(w => w.Salary).IsRequired()
                                           .HasDefaultValue(0);
            builder.Property(w => w.Premium).IsRequired()
                                            .HasDefaultValue(0);
            builder.Property(w => w.DateOfEmployment).IsRequired()
                                            .HasDefaultValue(DateTime.Now.AddHours(8));
            builder.Ignore(w => w.FullInfo);
            builder.Ignore(w => w.Experience);
        }
    }
}
