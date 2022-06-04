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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username).IsRequired()
                                 .HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired()
                                             .HasMaxLength(32);
            builder.Property(u => u.IsAdmin).IsRequired();
        }
    }
}
