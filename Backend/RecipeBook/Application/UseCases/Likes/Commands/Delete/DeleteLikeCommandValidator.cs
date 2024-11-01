using FluentValidation;

namespace Application.UseCases.Likes.Commands.Delete;

public class DeleteLikeCommandValidator : AbstractValidator<DeleteLikeCommand>
{
    public DeleteLikeCommandValidator()
    {

    }
}
