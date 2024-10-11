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

        builder.HasOne( f => f.User )
            .WithMany( u => u.Favorites )
            .HasForeignKey( "user_id" )
            .IsRequired()
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( f => f.Recipe )
            .WithMany( r => r.Favorites )
            .HasForeignKey( "recipe_id" )
            .IsRequired()
            .OnDelete( DeleteBehavior.Cascade );
    }
}