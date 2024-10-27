using Application.Common.CQRS.Command;
using Application.Common.FileHelper;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Commands.Dtos.Ingredients;
using Application.UseCases.Commands.Dtos.Steps;
using Application.UseCases.Commands.Dtos.Tags;
using Application.UseCases.Commands.Ingredients;
using Application.UseCases.Commands.Steps.Create;
using Application.UseCases.Commands.Tags.Create;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.UseCases.Commands.Recipes.Create;

public class CreateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork,
    ICommandHandler<CreateTagCommand, Tag> createTagHandler,
    ICommandHandler<CreateStepCommand, Step> createStepHandler,
    ICommandHandler<CreateIngredientCommand, Ingredient> createIngredientHandler,
    IValidator<CreateRecipeCommand> validator,
    IFileHelper fileHelper,
    IConfiguration configuration,
    IMapper mapper
) : ICommandHandler<CreateRecipeCommand>
{
    public async Task Handle( CreateRecipeCommand command )
    {
        ValidationResult validationResult = await validator.ValidateAsync( command );
        if ( !validationResult.IsValid )
        {
            throw new ValidationException( validationResult.Errors );
        }

        Recipe recipe = new Recipe( command.AuthorId,
            command.Name, command.Description,
            command.CookTime,
            command.PortionCount,
            command.ImageName
        );

        await AddTags( recipe, command.Tags );

        await AddSteps( recipe, command.Steps );

        await AddIngredients( recipe, command.Ingredients );

        await recipeRepository.Create( recipe );

        await unitOfWork.Commit();

        SaveImage( recipe, command.ImageFile );
    }

    private async Task AddTags( Recipe recipe, ICollection<TagDto> tags )
    {
        foreach ( TagDto tag in tags )
        {
            CreateTagCommand createTagCommand = mapper.Map<CreateTagCommand>( tag );
            Tag tagEntity = await createTagHandler.Handle( createTagCommand );
            recipe.Tags.Add( tagEntity );
        }
    }

    private async Task AddSteps( Recipe recipe, ICollection<StepDto> steps )
    {
        foreach ( StepDto step in steps )
        {
            CreateStepCommand createStepCommand = mapper.Map<CreateStepCommand>( step );
            createStepCommand.RecipeId = recipe.Id;
            Step stepEntity = await createStepHandler.Handle( createStepCommand );
            recipe.Steps.Add( stepEntity );
        }
    }

    private async Task AddIngredients( Recipe recipe, ICollection<IngredientDto> ingredients )
    {
        foreach ( IngredientDto ingredient in ingredients )
        {
            CreateIngredientCommand createIngredientCommand = mapper.Map<CreateIngredientCommand>( ingredient );
            createIngredientCommand.RecipeId = recipe.Id;
            Ingredient ingredientEntity = await createIngredientHandler.Handle( createIngredientCommand );
            recipe.Ingredients.Add( ingredientEntity );
        }
    }

    private void SaveImage( Recipe recipe, IFormFile image )
    {
        string fileExtension = Path.GetExtension( recipe.ImageName );
        string fileNameOnDisk = recipe.Id + fileExtension;
        fileHelper.Save( configuration.GetSection( "RecipeImages" ).Value, fileNameOnDisk, image.OpenReadStream() );
    }
}
