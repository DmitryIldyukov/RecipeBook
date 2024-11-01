using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandValidator( IUserRepository repo )
    {
        _repository = repo;

        RuleFor( x => x.UserId )
            .NotEmpty().WithMessage( "Идентификатор автора обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Имя автора не может быть пустым" );

        RuleFor( command => command.Login )
            .NotEmpty().WithMessage( "Логин не может быть пустым." )
            .MaximumLength( 30 ).WithMessage( "Максимальная длина логина 30 символов." )
            .MustAsync( LoginIsUnique ).WithMessage( "Пользователь с таким логином уже существует." );

        RuleFor( command => command.Password )
            .NotEmpty()
            .When( c => !string.IsNullOrEmpty( c.Password ) )
            .WithMessage( "Пароль не может быть пустым." )
            .MinimumLength( 8 )
            .When( c => !string.IsNullOrEmpty( c.Password ) )
            .WithMessage( "Пароль должен состоять минимум из 8 символов." );

        RuleFor( command => command.Information )
            .MaximumLength( 255 );
    }

    private async Task<bool> LoginIsUnique( UpdateUserCommand command, string login, CancellationToken cancellationToken )
    {
        return !await _repository.ContainsAsync( u => u.Login == login && u.Id != command.UserId );
    }
}
