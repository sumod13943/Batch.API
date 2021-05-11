using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI.Model
{
    public class BatchContext : DbContext
    {
        public BatchContext(DbContextOptions<BatchContext> options) : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<ACL> ACLs { get; set; }
        public DbSet<Attributes> Attributes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var splitStringConverter = new ValueConverter<IList<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            builder.Entity<ACL>().Property(e => e.ReadUsers).HasConversion(splitStringConverter);//.Metadata.SetValueComparer(valueComparer);
            builder.Entity<ACL>().Property(e => e.ReadGroups).HasConversion(splitStringConverter);//.Metadata.SetValueComparer(valueComparer);

            // Configure the primary key for BaseCard
            builder.Entity<Batch>().HasKey(t => t.BatchId);
        }

    }
}
