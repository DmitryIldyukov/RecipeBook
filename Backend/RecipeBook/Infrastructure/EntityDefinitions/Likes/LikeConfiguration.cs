using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Likes;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure( EntityTypeBuilder<Like> builder )
    {
        builder.ToTable( "likes" )
            .HasKey( l => l.Id );

        builder.Property( l => l.Id )
            .HasColumnName( "like_id" )
            .ValueGeneratedOnAdd();

        builder.Property( l => l.UserId )
            .HasColumnName( "user_id" );

        builder.Property( l => l.RecipeId )
            .HasColumnName( "recipe_id" );

        builder.Property( l => l.CreatedAt )
            .HasColumnName( "created_at" );

        builder.HasOne( f => f.User )
            .WithMany( u => u.Likes )
            .HasForeignKey( f => f.UserId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( f => f.Recipe )
            .WithMany( r => r.Likes )
            .HasForeignKey( f => f.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
