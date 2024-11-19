using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Recipes.Queries.GetById;

public class GetRecipeByIdQueryHandler(
    IRecipeRepository recipeRepository,
    IValidator<GetRecipeByIdQuery> validator,
    IMapper mapper
) : IQueryHandler<GetRecipeByIdQuery, ResultT<GetRecipeQueryDto>>
{
    public async Task<ResultT<GetRecipeQueryDto>> Handle( GetRecipeByIdQuery query )
    {
        ValidationResult validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsValid )
        {
            return ResultT<GetRecipeQueryDto>.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        Recipe recipe = await recipeRepository.GetById( query.RecipeId );


        if ( query.UserId is not null )
        {
            bool isLiked = recipe.Likes.Any( l => l.UserId == query.UserId );
            bool isFavorite = recipe.Favorites.Any( l => l.UserId == query.UserId );
            return ResultT<GetRecipeQueryDto>.Success( mapper.Map<GetRecipeQueryDto>( recipe )
                with
            { IsLiked = isLiked, IsFavorite = isFavorite }, "Рецепт найден." );
        }

        return ResultT<GetRecipeQueryDto>.Success( mapper.Map<GetRecipeQueryDto>( recipe ), "Рецепт найден." );
    }

    private async Task<ResultT<GetRecipeQueryDto>> ValidateAsync( GetRecipeByIdQuery query, Recipe recipe )
    {
        

        if ( recipe is null )
        {
            return ResultT<GetRecipeQueryDto>.Fail( $"Рецепт с Id {query.RecipeId} не найден." );
        }

        return ResultT<GetRecipeQueryDto>.Success( null );
    }
}
