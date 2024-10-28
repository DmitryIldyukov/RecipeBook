using Application.UseCases.Commands.Recipes.Dtos;
using Application.UseCases.Commands.Steps.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class StepProfile : Profile
{
    public StepProfile()
    {
        CreateMap<RecipeStepDto, CreateStepCommand>();
        CreateMap<RecipeStepDto, Step>();
        CreateMap<CreateStepCommand, Step>();
    }
}
