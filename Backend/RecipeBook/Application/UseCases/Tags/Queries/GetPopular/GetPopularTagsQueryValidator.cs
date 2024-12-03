using FluentValidation;

namespace Application.UseCases.Tags.Queries.GetPopular
{
    public class GetPopularTagsQueryValidator : AbstractValidator<GetPopularTagsQuery>
    {
        public GetPopularTagsQueryValidator()
        {
            RuleFor( q => q.Count ).NotEmpty().GreaterThan( 0 ).WithMessage( "Запрашевоемое количество тегов должно быть больше 0." );
        }
    }
}
