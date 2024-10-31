using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Recipes.Dtos;

namespace Application.UseCases.Recipes.Queries.GetById;

public class GetRecipeByIdQueryHandler : IQueryHandler<GetRecipeByIdQuery, ResultT<DailyRecipeDto>>
{
    public Task<ResultT<DailyRecipeDto>> Handle( GetRecipeByIdQuery query )
    {
        throw new NotImplementedException();
    }
}
