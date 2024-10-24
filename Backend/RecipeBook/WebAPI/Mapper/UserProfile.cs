using Application.UseCases.Commands.Users.Create;
using Application.UseCases.Commands.Users.Update;
using AutoMapper;
using WebAPI.Dtos.User;

namespace WebAPI.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegisterDto, CreateUserCommand>();
        CreateMap<UserEditDto, UpdateUserCommand>();
    }
}
