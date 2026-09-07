using TARge25Shop.Core.Domain;

namespace TARge25Shop.Data;
using TARge25Shop.Core.Domain;

using Microsoft.EntityFrameworkCore;

public class TARge25ShopContext : DbContext
{
    public TARge25ShopContext(DbContextOptions<TARge25ShopContext> options) : base(options)
    {}
    
    public DbSet<Spaceship> Spaceships { get; set; }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Spaceship>()
    // }
}