﻿using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Commands.User.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _repo;

    public CreateUserCommandValidator( IUserRepository repo )
    {
        _repo = repo;

        RuleFor( command => command.Name )
            .NotEmpty().WithMessage( "Имя автора не может быть пустым" );

        RuleFor( command => command.Login )
            .NotEmpty().WithMessage( "Логин не может быть пустым." )
            .MustAsync( LoginIsUnique ).WithMessage( "Пользователь с таким логином уже существует." );

        RuleFor( command => command.Password )
            .NotEmpty().WithMessage( "Пароль не может быть пустым." )
            .MinimumLength( 8 ).WithMessage( "Пароль должен состоять минимум из 8 символов." );
    }

    private async Task<bool> LoginIsUnique( string login, CancellationToken cancellationToken )
    {
        return !await _repo.UserIsExist( login );
    }
}
