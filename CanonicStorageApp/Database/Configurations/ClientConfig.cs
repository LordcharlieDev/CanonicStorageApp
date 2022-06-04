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
    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(c => c.FullName).IsRequired()
                                             .HasMaxLength(200);
            builder.Property(c => c.Address).IsRequired()
                                            .HasMaxLength(300);
            builder.Property(c => c.Email).IsRequired()
                                          .HasMaxLength(100);
            builder.Property(c => c.Phone).IsRequired()
                                          .HasMaxLength(13);
        }
    }
}
