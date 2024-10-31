using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Tags;

public class TagRepository( RecipeBookDbContext dbContext ) : ITagRepository
{
    public async Task Create( Tag tag )
    {
        await dbContext.Tags.AddAsync( tag );
    }

    public void Delete( Tag tag )
    {
        dbContext.Tags.Remove( tag );
    }

    public IQueryable<Tag> GetAll()
    {
        return dbContext.Tags;
    }

    public async Task<Tag> GetByName( string name )
    {
        return await dbContext.Tags.FirstOrDefaultAsync( t => t.Name == name );
    }

    public async Task<bool> IsUsedInOtherRecipes( int tagId, int recipeId )
    {
        var tag = await dbContext.Tags
            .Include( t => t.Recipes )
            .FirstOrDefaultAsync( t => t.Id == tagId );

        if ( tag == null )
        {
            return false;
        }

        return tag.Recipes.Any( r => r.Id != recipeId );
    }
}
