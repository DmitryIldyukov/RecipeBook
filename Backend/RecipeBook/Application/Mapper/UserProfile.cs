using Application.UseCases.Users.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserQueryDto>()
            .ForMember( dest => dest.RecipesCount, opt => opt.MapFrom( src => src.Recipes.Count() ) )
            .ForMember( dest => dest.FavoritesCount, opt => opt.MapFrom( src => src.Favorites.Count() ) )
            .ForMember( dest => dest.LikesCount, opt => opt.MapFrom( src => src.Likes.Count() ) );
    }
}
