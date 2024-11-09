using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using AutoMapper;
using Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.UseCases.Recipes.Queries.GetDailyRecipe;

public class GetDailyRecipeQueryHandler(
    IRecipeRepository recipeRepository,
    IMapper mapper
) : IQueryHandler<GetDailyRecipeQuery, ResultT<DailyRecipeDto>>
{
    public async Task<ResultT<DailyRecipeDto>> Handle( GetDailyRecipeQuery query )
    {
        Recipe dailyRecipe = await recipeRepository.GetDailyRecipe();

        if ( dailyRecipe is null )
        {
            return ResultT<DailyRecipeDto>.Fail( "Рецепт дня не найден." );
        }

        return ResultT<DailyRecipeDto>.Success( mapper.Map<DailyRecipeDto>( dailyRecipe ), "Рецепт дня найден." );
    }
}
