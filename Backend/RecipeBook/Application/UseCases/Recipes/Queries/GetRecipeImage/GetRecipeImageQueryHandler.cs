using Application.Common.CQRS.Query;
using Application.Common.FileHelper;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Recipes.Queries.GetRecipeImage;

public class GetRecipeImageQueryHandler(
    IRecipeRepository recipeRepository,
    IConfiguration configuration,
    IFileHelper fileHelper,
    IValidator<GetRecipeImageQuery> validator
) : IQueryHandler<GetRecipeImageQuery, ResultT<GetImageQueryDto>>
{
    public async Task<ResultT<GetImageQueryDto>> Handle( GetRecipeImageQuery query )
    {
        Recipe recipe = await recipeRepository.GetById( query.RecipeId );

        ResultT<GetImageQueryDto> validationResult = await ValidateAsync( query, recipe );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        string fullPath = BuildImagePath( recipe );

        FileData file = fileHelper.Get( fullPath );

        GetImageQueryDto response = new GetImageQueryDto( file.File, file.MimeType, recipe.ImageName );

        return ResultT<GetImageQueryDto>.Success( response, "Изображение успешно сохранено." );
    }

    private string BuildImagePath( Recipe recipe )
    {
        string fileName = $"{recipe.Id}{Path.GetExtension( recipe.ImageName )}";
        string storagePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            configuration.GetSection( "RecipeImages" ).Value
        );

        return Path.Combine( storagePath, fileName );
    }

    private async Task<ResultT<GetImageQueryDto>> ValidateAsync( GetRecipeImageQuery query, Recipe recipe )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<GetImageQueryDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        if ( recipe is null )
        {
            return ResultT<GetImageQueryDto>.Fail( "Рецепт не найден." );
        }

        return ResultT<GetImageQueryDto>.Success( null );
    }
}
