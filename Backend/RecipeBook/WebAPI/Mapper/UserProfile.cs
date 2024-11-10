using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Update;
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
