using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Favorites;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure( EntityTypeBuilder<Favorite> builder )
    {
        builder.ToTable( "favorites" )
            .HasKey( x => x.Id );

        builder.Property( f => f.Id )
            .HasComment( "Id избранного" )
            .HasColumnName( "favorite_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( f => f.UserId )
            .HasComment( "Id пользователя" )
            .HasColumnName( "user_id" )
            .IsRequired();

        builder.Property( f => f.RecipeId )
            .HasComment( "Id рецепта" )
            .HasColumnName( "recipe_id" )
            .IsRequired();

        builder.HasOne( f => f.User )
            .WithMany( u => u.Favorites )
            .HasForeignKey( u => u.UserId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( f => f.Recipe )
            .WithMany( r => r.Favorites )
            .HasForeignKey( r => r.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}