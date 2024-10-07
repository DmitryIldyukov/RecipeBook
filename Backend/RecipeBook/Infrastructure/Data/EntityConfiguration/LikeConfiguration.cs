using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure( EntityTypeBuilder<Like> builder )
    {
        builder.ToTable( "likes" )
            .HasKey( l => l.LikeId );

        builder.Property( l => l.LikeId )
            .HasComment( "Id лайка" )
            .HasColumnName( "like_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( l => l.UserId )
            .HasComment( "Id пользователя" )
            .HasColumnName( "user_id" )
            .IsRequired();

        builder.Property( l => l.RecipeId )
            .HasComment( "Id рецепта" )
            .HasColumnName( "recipe_id" )
            .IsRequired();

        builder.Property( l => l.CreatedAt )
            .HasComment( "Дата и время лайка" )
            .HasColumnName( "created_at" )
            .IsRequired();

        builder.HasOne( f => f.User )
            .WithMany( u => u.Likes )
            .HasForeignKey( u => u.UserId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( f => f.Recipe )
            .WithMany( r => r.Likes )
            .HasForeignKey( r => r.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
