using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext( DbContextOptions<RecipeBookDbContext> options )
        : base( options )
    { }

    #region DbSets

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Tag> Tags { get; set; }

    #endregion

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfigurationsFromAssembly( GetType().Assembly );
    }
}
