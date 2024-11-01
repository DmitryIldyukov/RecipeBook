using FluentValidation;

namespace Application.UseCases.Likes.Commands.Create;

public class CreateLikeCommandValidator : AbstractValidator<CreateLikeCommand>
{
    public CreateLikeCommandValidator()
    {

    }
}
