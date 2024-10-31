using Application.Common.CQRS.Command;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.UseCases.Ingredients.Commands.Create;
using Application.UseCases.Ingredients.Commands.UpdateIngredient;
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
    ICommandHandler<CreateStepCommand, ResultT<Step>> createStepHandler
) : ICommandHandler<UpdateRecipeStepsCommand, Result>
{
    public async Task<Result> Handle( UpdateRecipeStepsCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            return Result.Failure( validationResult.Errors.Select( e => e.ErrorMessage ) );
        }

        ICollection<Step> recipeSteps = command.Recipe.Steps;
        List<Step> stepsToRemove = new List<Step>();

        foreach ( Step step in recipeSteps )
        {
            bool existsInCommand = command.Steps.Any( s => s.StepId is not null && s.StepId == step.Id );
            if ( !existsInCommand )
            {
                stepsToRemove.Add( step );
            }
        }

        foreach ( Step step in stepsToRemove )
        {
            recipeSteps.Remove( step );
            stepRepository.Delete( step );
        }

        foreach ( RecipeStepDto stepDto in command.Steps )
        {
            if ( stepDto.StepId is not null )
            {
                Step stepEntity = recipeSteps.FirstOrDefault( i => i.Id == stepDto.StepId );

                if ( stepEntity is null )
                {
                    return Result.Failure( $"Шаг с Id {stepDto.StepId} не найден." );
                }

                UpdateStepCommand updateStepCommand = new UpdateStepCommand()
                {
                    StepId = stepEntity.Id,
                    Description = stepDto.Description
                };

                Result updateIngredientResult = await updateStepHandler.Handle( updateStepCommand );
                if ( !updateIngredientResult.IsSuccess )
                {
                    return Result.Failure( updateIngredientResult.ErrorMessages );
                }
            }
            else
            {
                CreateStepCommand createStepCommand = new CreateStepCommand()
                {
                    RecipeId = command.Recipe.Id,
                    Description = stepDto.Description
                };

                ResultT<Step> createStepResult = await createStepHandler.Handle( createStepCommand );
                if ( !createStepResult.IsSuccess )
                {
                    return Result.Failure( createStepResult.ErrorMessages );
                }

                recipeSteps.Add( createStepResult.Value );
            }
        }

        return Result.Success();
    }
}
