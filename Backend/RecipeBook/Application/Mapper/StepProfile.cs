using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Steps.Commands.Create;
using Application.UseCases.Steps.Dtos;
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
        CreateMap<Step, GetStepDto>();
    }
}
