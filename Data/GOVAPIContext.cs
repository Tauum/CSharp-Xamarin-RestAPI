using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GOVAPI.Models;

namespace GOVAPI.Data
{
    public class GOVAPIContext : DbContext
    {
        public GOVAPIContext (DbContextOptions<GOVAPIContext> options) : base(options) { }
        public DbSet<GOVAPI.Models.Product> Product { get; set; }
        public DbSet<GOVAPI.Models.Review> Review { get; set; }
        public DbSet<GOVAPI.Models.User> User { get; set; }
        public DbSet<GOVAPI.Models.Image> Image { get; set; }
        public DbSet<GOVAPI.Models.Category> Category { get; set; }
        public DbSet<GOVAPI.Models.UserProduct> UserProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //needed for the userproudcts
        {
            modelBuilder.Entity<User>().HasMany(x => x.UserProducts)
                                       .WithOne(x => x.User)
                                       .HasForeignKey(x => x.UserID);
            modelBuilder.Entity<User>().Ignore(x => x.ScoreTotal); //causes error without this
        }
    }
}
