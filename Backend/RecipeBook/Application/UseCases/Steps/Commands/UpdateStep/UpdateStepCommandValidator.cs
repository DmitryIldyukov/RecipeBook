using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.UseCases.Steps.Commands.UpdateStep;

public class UpdateStepCommandValidator : AbstractValidator<UpdateStepCommand>
{
    public UpdateStepCommandValidator()
    {
        RuleFor( s => s.StepId )
            .NotNull().WithMessage( "Id шага обязателен." )
            .GreaterThan( 0 ).WithMessage( "Идентификатор должен быть положительным числом." );

        RuleFor( s => s.Description )
            .NotEmpty().WithMessage( "Описание шага обязательно." );
    }
}
