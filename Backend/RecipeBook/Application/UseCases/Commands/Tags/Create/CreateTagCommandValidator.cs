using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Commands.Tags.Create;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    private readonly ITagRepository _repository;

    public CreateTagCommandValidator( ITagRepository repo )
    {
        _repository = repo;

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Тэг не может быть пустым." );
    }
}