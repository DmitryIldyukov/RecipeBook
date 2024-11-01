using Application.UseCases.Favorites.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class FavoriteProfile : Profile
{
    public FavoriteProfile()
    {
        CreateMap<CreateFavoriteCommand, Favorite>();
    }
}
