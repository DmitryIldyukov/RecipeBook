using Application.Interfaces;

namespace Infrastructure.Data;

public class UnitOfWork( RecipeBookDbContext dbContext ) : IUnitOfWork
{
    public Task Commit()
    {
        return dbContext.SaveChangesAsync();
    }
}
