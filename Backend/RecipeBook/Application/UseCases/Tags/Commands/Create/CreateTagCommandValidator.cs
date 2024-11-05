using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Tags.Commands.Create;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    private readonly ITagRepository _repository;

    public CreateTagCommandValidator( ITagRepository repo )
    {
        _repository = repo;

        RuleFor( s => s.Recipe )
            .NotNull().WithMessage( "Рецепт обязателен." );

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Тэг не может быть пустым." )
            .MaximumLength( 20 ).WithMessage( "Максимальная длина тэга 30 символов." );
    }
}