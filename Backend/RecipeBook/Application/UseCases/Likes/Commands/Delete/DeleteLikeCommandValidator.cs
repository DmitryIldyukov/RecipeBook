using FluentValidation;

namespace Application.UseCases.Likes.Commands.Delete;

public class DeleteLikeCommandValidator : AbstractValidator<DeleteLikeCommand>
{
    public DeleteLikeCommandValidator()
    {
        RuleFor( f => f.LikeId )
            .NotEmpty().WithMessage( "Идентификатор лайка обязателен." );

        RuleFor( f => f.UserId )
            .NotEmpty().WithMessage( "Идентификатор пользователя обязателен." );
    }
}
