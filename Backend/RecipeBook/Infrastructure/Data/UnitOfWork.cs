using Application.Interfaces;

namespace Infrastructure.Data;

public class UnitOfWork( RecipeBookDbContext context ) : IUnitOfWork
{
    private readonly RecipeBookDbContext _context = context;

    public Task Commit()
    {
        return _context.SaveChangesAsync();
    }
}
