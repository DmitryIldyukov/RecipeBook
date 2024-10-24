using Application.UseCases.Commands.Dtos.Steps;
using Application.UseCases.Commands.Steps.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class StepProfile : Profile
{
    public StepProfile()
    {
        CreateMap<StepDto, CreateStepCommand>();
        CreateMap<StepDto, Step>();
        CreateMap<CreateStepCommand, Step>();
    }
}
