using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Tags;

public class TagRepository( RecipeBookDbContext dbContext ) : ITagRepository
{
    public async Task<bool> ContainsAsync( Expression<Func<Tag, bool>> predicate )
    {
        return await dbContext.Tags.AnyAsync( predicate );
    }

    public async Task Create( Tag entity )
    {
        await dbContext.Tags.AddAsync( entity );
    }

    public async Task<IQueryable<Tag>> GetAll()
    {
        return dbContext.Tags;
    }

    public async Task<Tag> GetByName( string name )
    {
        return await dbContext.Tags.FirstOrDefaultAsync( t => t.Name == name );
    }
}
