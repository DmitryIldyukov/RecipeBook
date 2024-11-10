using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Tags;

public class TagRepository( RecipeBookDbContext dbContext ) : ITagRepository
{
    public async Task Create( Tag entity )
    {
        await dbContext.Tags.AddAsync( entity );
    }

    public IQueryable<Tag> GetAll()
    {
        return dbContext.Tags;
    }

    public async Task<Tag> GetByName( string name )
    {
        return await dbContext.Tags.FirstOrDefaultAsync( t => t.Name == name );
    }
}
