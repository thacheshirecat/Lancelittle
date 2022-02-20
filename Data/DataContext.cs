using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lancelittle.Models;
using Microsoft.EntityFrameworkCore;

namespace Lancelittle.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<CharClass> Classes { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Cantrip> Cantrips { get; set; }
        public DbSet<Relic> Relics { get; set; }
        public DbSet<CharacterCantrip> CharacterCantrips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterCantrip>()
                .HasKey(cc => new { cc.CharacterId, cc.CantripId});
        }
    }
}