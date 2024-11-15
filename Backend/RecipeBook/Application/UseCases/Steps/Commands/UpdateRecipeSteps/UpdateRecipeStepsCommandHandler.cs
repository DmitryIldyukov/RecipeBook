using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Steps.Commands.Create;
using Application.UseCases.Steps.Commands.UpdateStep;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Application.UseCases.Steps.Commands.UpdateRecipeSteps;

public class UpdateRecipeStepsCommandHandler(
    IStepRepository stepRepository,
    IValidator<UpdateRecipeStepsCommand> validator,
    ICommandHandler<UpdateStepCommand, Result> updateStepHandler,
    ICommandHandler<CreateStepCommand, Result> createStepHandler
) : ICommandHandler<UpdateRecipeStepsCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeStepsCommand command )
    {
        Result validationResult = await ValidateCommandAsync( command );
        if ( !validationResult.IsSuccess )
        {
            return validationResult;
        }

        ICollection<Step> recipeSteps = command.Recipe.Steps;
        List<Step> stepsToRemove = IdentifyStepsToRemove( recipeSteps, command.Steps );
        RemoveStepsAsync( stepsToRemove, recipeSteps );

        Result updateResult = await UpdateAndCreateStepsAsync( command, recipeSteps );
        if ( !updateResult.IsSuccess )
        {
            return updateResult;
        }

        return Result.Success();
    }

    private List<Step> IdentifyStepsToRemove( ICollection<Step> recipeSteps, ICollection<RecipeStepDto> commandSteps )
    {
        return recipeSteps
            .Where( step => !commandSteps.Any( s => s.StepId == step.Id ) )
            .ToList();
    }

    private void RemoveStepsAsync( IEnumerable<Step> stepsToRemove, ICollection<Step> recipeSteps )
    {
        foreach ( Step step in stepsToRemove )
        {
            recipeSteps.Remove( step );
            stepRepository.Delete( step );
        }
    }

    private async Task<Result> UpdateAndCreateStepsAsync( UpdateRecipeStepsCommand command, ICollection<Step> recipeSteps )
    {
        foreach ( RecipeStepDto step in command.Steps )
        {
            if ( step.StepId is not null )
            {
                Result result = await UpdateExistingStepAsync( step, recipeSteps );
                if ( !result.IsSuccess )
                {
                    return result;
                }
            }
            else
            {
                Result result = await CreateNewStepAsync( step, command.Recipe );
                if ( !result.IsSuccess )
                {
                    return result;
                }
            }
        }

        return Result.Success();
    }

    private async Task<Result> UpdateExistingStepAsync( RecipeStepDto stepDto, ICollection<Step> recipeSteps )
    {
        Step step = recipeSteps.FirstOrDefault( i => i.Id == stepDto.StepId );
        if ( step == null )
        {
            return Result.Fail( $"Шаг с Id {stepDto.StepId} не найден." );
        }

        UpdateStepCommand updateStepCommand = new UpdateStepCommand
        {
            StepId = step.Id,
            Description = stepDto.Description
        };
        Result updateResult = await updateStepHandler.Handle( updateStepCommand );

        return updateResult.IsSuccess ? Result.Success() : Result.Fail( updateResult.ErrorMessages );
    }

    private async Task<Result> CreateNewStepAsync( RecipeStepDto stepDto, Recipe recipe )
    {
        CreateStepCommand createStepCommand = new CreateStepCommand
        {
            Recipe = recipe,
            Description = stepDto.Description
        };
        Result createResult = await createStepHandler.Handle( createStepCommand );

        return createResult.IsSuccess ? Result.Success() : Result.Fail( createResult.ErrorMessages );
    }

    private async Task<Result> ValidateCommandAsync( UpdateRecipeStepsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Fail( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        return Result.Success();
    }
}