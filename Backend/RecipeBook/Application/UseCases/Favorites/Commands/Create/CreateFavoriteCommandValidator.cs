using FluentValidation;

namespace Application.UseCases.Favorites.Commands.Create;

public class CreateFavoriteCommandValidator : AbstractValidator<CreateFavoriteCommand>
{
    public CreateFavoriteCommandValidator()
    {

    }
}
