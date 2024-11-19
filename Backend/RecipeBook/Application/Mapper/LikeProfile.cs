using Application.UseCases.Likes.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class LikeProfile : Profile
{
    public LikeProfile()
    {
        CreateMap<CreateLikeCommand, Like>();
    }
}
