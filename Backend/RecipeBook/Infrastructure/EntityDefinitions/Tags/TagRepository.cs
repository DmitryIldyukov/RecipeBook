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

    public async Task<IReadOnlyList<Tag>> GetAll()
    {
        return await dbContext.Tags.ToListAsync();
    }

    public async Task<Tag> GetByName( string name )
    {
        return await dbContext.Tags.FirstOrDefaultAsync( t => t.Name == name );
    }

    public async Task<IReadOnlyList<Tag>> GetPopularTags( int count )
    {
        if ( count < 0 )
        {
            return new List<Tag>();
        }

        return await dbContext.Tags
            .Include( t => t.Recipes )
            .OrderByDescending( t => t.Recipes.Count() )
            .Take( count )
            .ToListAsync();
    }

    public async Task<bool> IsUsedInMultipleRecipes( int tagId )
    {
        Tag tag = await dbContext.Tags
            .Include( t => t.Recipes )
            .FirstOrDefaultAsync( t => t.Id == tagId );

        return tag != null && tag.Recipes.Count() > 1;
    }
}
