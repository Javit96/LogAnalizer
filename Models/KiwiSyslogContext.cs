using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LogAudit.Models
{
    public partial class KiwiSyslogContext : DbContext
    {
        public KiwiSyslogContext()
        {
        }

        public KiwiSyslogContext(DbContextOptions<KiwiSyslogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Syslogd> Syslogds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

                var secretProvider = config.Providers.First();
                
                optionsBuilder.UseSqlServer();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Syslogd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Syslogd");

                entity.Property(e => e.MsgDate)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MsgHostname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MsgPriority)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MsgText)
                    .HasMaxLength(4096)
                    .IsUnicode(false);

                entity.Property(e => e.MsgTime)
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
