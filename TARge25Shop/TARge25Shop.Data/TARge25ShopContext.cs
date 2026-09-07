namespace TARge25Shop.Data;

using Microsoft.EntityFrameworkCore;

public class TARge25ShopContext : DbContext
{
    public TARge25ShopContext(DbContextOptions<TARge25ShopContext> options) : base(options)
    {}
    
}