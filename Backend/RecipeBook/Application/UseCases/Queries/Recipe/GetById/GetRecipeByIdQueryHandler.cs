using Application.Common.CQRS.Query;
using Application.UseCases.Queries.Recipe.Dtos;

namespace Application.UseCases.Queries.Recipe.GetById;

public class GetRecipeByIdQueryHandler : IQueryHandler<GetRecipeByIdQuery, GetRecipeQueryDto>
{
    public Task<GetRecipeQueryDto> Handle( GetRecipeByIdQuery query )
    {
        throw new NotImplementedException();
    }
}
