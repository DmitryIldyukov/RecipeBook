using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Recipes.Dtos;

namespace Application.UseCases.Recipes.Queries.GetById;

public class GetRecipeByIdQueryHandler : IQueryHandler<GetRecipeByIdQuery, ResultT<GetRecipeQueryDto>>
{
    public Task<ResultT<GetRecipeQueryDto>> Handle( GetRecipeByIdQuery query )
    {
        throw new NotImplementedException();
    }
}
