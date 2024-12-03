using Application.Interfaces.Repositories;
using Application.UseCases.Favorites.Commands.Delete;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandValidator( IUserRepository userRepository )
    {
        _userRepository = userRepository;

        RuleFor( x => x.UserId )
            .NotEmpty().WithMessage( "Идентификатор автора обязателен" )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом" )
            .MustAsync( UserExists ).WithMessage( r => $"Пользователь с Id {r.UserId} не найден." );

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Имя автора не может быть пустым" );

        RuleFor( command => command.Login )
            .NotEmpty().WithMessage( "Логин не может быть пустым" )
            .MaximumLength( 30 ).WithMessage( "Максимальная длина логина 30 символов" )
            .MustAsync( LoginIsUnique ).WithMessage( "Пользователь с таким логином уже существует" );

        RuleFor( command => command.Password )
            .Must( password => string.IsNullOrEmpty( password ) || password.Length >= 8 )
            .WithMessage( "Пароль должен состоять минимум из 8 символов, если указан." );

        RuleFor( command => command.Information )
            .MaximumLength( 255 );
    }

    private async Task<bool> LoginIsUnique( UpdateUserCommand command, string login, CancellationToken cancellationToken )
    {
        return !await _userRepository.ContainsAsync( u => u.Login == login && u.Id != command.UserId );
    }

    private async Task<bool> UserExists( int userId, CancellationToken cancellationToken )
    {
        return await _userRepository.ContainsAsync( u => u.Id == userId );
    }
}
