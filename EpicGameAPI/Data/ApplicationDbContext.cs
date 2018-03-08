using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicGameAPI.Models;
using EpicGameAPI.Controllers;

namespace EpicGameAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Adventure> Adventure { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AdventureChoiceHistory> AdventureChoiceHistory { get; set; }
        public DbSet<Character> Character { get; set; }
        public DbSet<Enemy> Enemy { get; set; }
        public DbSet<PathOption> PathOption { get; set; }
        public DbSet<RoadBlock> RoadBlock { get; set; }
        public DbSet<StoryPath> StoryPath { get; set; }
        public DbSet<UnitClass> UnitClass { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
