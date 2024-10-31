using FluentValidation;

namespace Application.UseCases.Recipes.Queries.GetDailyRecipe;

public class GetDailyRecipeQueryValidator : AbstractValidator<GetDailyRecipeQuery>
{
    public GetDailyRecipeQueryValidator()
    {

    }
}
