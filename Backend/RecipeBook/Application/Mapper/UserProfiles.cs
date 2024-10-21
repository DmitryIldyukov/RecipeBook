using Application.UseCases.Queries.Users.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<User, GetUserQueryDto>();
    }
}
