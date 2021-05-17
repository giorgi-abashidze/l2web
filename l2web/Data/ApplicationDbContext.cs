using l2web.Data.DataModels;
using l2web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace l2web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<DataUpdate> DataUpdates { get; set; }
        public DbSet<CastleCache> CastleCache { get; set; }
        public DbSet<Account> GameAccount { get; set; }
        public DbSet<CharacterCache> CharacterCache { get; set; }
        public DbSet<ClanCache> ClanCache { get; set; }
        public DbSet<EpicOwnersCache> EpicOwnersCache { get; set; }

        public DbSet<OnlineCache> OnlineCache { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OnlineCache>()
                .HasKey(c => new { c.Online, c.TDate });

            builder.Entity<EpicOwnersCache>()
                .HasKey(c => new { c.CharName, c.ItemId });


            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Account)
                .WithMany()
                .HasForeignKey(k => k.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
           .Property(b => b.LastAccountUpdateTime)
           .HasDefaultValue(DateTime.Now.AddDays(-1));

            builder.Entity<Account>()
                .HasMany(a => a.Characters)
                .WithOne(a => a.Account)
                .HasForeignKey(k => k.AccountId);

            builder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(k => k.userId);


            builder.Entity<DataUpdate>()
            .Property(b => b.LastDataUpdate)
            .HasDefaultValue(DateTime.Now.AddDays(-1));

            builder.Entity<DataUpdate>()
           .Property(b => b.LastOnlineUpdate)
           .HasDefaultValue(DateTime.Now.AddDays(-1));
        }
    }
}
