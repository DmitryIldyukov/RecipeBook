using Application.UseCases.Queries.Users.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserQueryDto>();
    }
}
