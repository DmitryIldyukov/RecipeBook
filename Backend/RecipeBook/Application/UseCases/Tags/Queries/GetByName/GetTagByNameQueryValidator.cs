using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Tags.Queries.GetByName;

public class GetTagByNameQueryValidator : AbstractValidator<GetTagByNameQuery>
{
    private readonly ITagRepository _tagRepository;

    public GetTagByNameQueryValidator( ITagRepository tagRepository )
    {
        _tagRepository = tagRepository;

        RuleFor( f => f.Tag )
           .NotEmpty().WithMessage( "Тег обязателен." )
           .MustAsync( TagExists ).WithMessage( "Тег не найден." );
    }

    private async Task<bool> TagExists( string tag, CancellationToken cancellationToken )
    {
        return await _tagRepository.GetByName( tag ) is not null;
    }
}
