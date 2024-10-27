using Application.Common.CQRS.Query;
using Application.Common.FileHelper;
using Application.Interfaces.Repositories;
using Application.UseCases.Queries.Recipes.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Queries.Recipes.GetRecipeImage;

public class GetRecipeImageQueryHandler(
    IRecipeRepository recipeRepository,
    IConfiguration configuration,
    IFileHelper fileHelper,
    IValidator<GetRecipeImageQuery> validator
) : IQueryHandler<GetRecipeImageQuery, GetImageQueryDto>
{
    public async Task<GetImageQueryDto> Handle( GetRecipeImageQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        Recipe recipe = await recipeRepository.GetById( query.RecipeId )
             ?? throw new NotFoundException( "Рецепт не найден." );

        var fullPath = BuildImagePath( recipe );

        FileData file = fileHelper.Get( fullPath );

        return new GetImageQueryDto( file.File, file.MimeType, recipe.ImageName );
    }

    private string BuildImagePath( Recipe recipe )
    {
        var fileName = $"{recipe.Id}{Path.GetExtension( recipe.ImageName )}";
        var storagePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            configuration.GetSection( "RecipeImages" ).Value
        );

        return Path.Combine( storagePath, fileName );
    }
}
