using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext( DbContextOptions<RecipeBookDbContext> options )
        : base( options )
    { }

    #region DbSets

    #endregion

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfigurationsFromAssembly( GetType().Assembly );
    }
}
