using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Fodder> Fodders { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Feed> Feeds { get; set; }

        public AppDbContext()
        {
            SQLitePCL.Batteries_V2.Init();

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Xamarin2.db3");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feed>()
                .HasOne(x => x.Fodder)
                .WithMany();

            modelBuilder.Entity<Pet>()
                .HasMany(x => x.Feeds)
                .WithOne(x => x.Pet);
            modelBuilder.Entity<Feed>()
                .HasOne(x => x.Pet)
                .WithMany(x => x.Feeds);
        }
    }
}
