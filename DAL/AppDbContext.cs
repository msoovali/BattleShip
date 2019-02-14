using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Game>()
                .Property(g => g.PlayerOneBoard)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<List<List<CellState>>>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
            
            modelBuilder
                .Entity<Game>()
                .Property(g => g.PlayerTwoBoard)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<List<List<CellState>>>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));

            modelBuilder
                .Entity<Game>()
                .Property(g => g.PlayerOneTurn)
                .HasConversion<int>();
            
            modelBuilder
                .Entity<Game>()
                .Property(g => g.ShipsCanTouch)
                .HasConversion<int>();
            
            modelBuilder
                .Entity<Game>()
                .Property(g => g.Ai)
                .HasConversion<int>();
            
            modelBuilder
                .Entity<Game>()
                .Property(g => g.GameOver)
                .HasConversion<int>();
            
            modelBuilder
                .Entity<Game>()
                .Property(g => g.GameShips)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<List<Ship>>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }
    }
}