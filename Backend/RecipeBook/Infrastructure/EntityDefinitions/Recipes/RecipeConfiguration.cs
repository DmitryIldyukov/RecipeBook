using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Recipes;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure( EntityTypeBuilder<Recipe> builder )
    {
        builder.ToTable( "recipes" )
            .HasKey( r => r.Id );

        builder.Property( r => r.Id )
            .HasColumnName( "recipe_id" )
            .ValueGeneratedOnAdd();

        builder.Property( r => r.AuthorId )
            .HasColumnName( "author_id" );

        builder.Property( r => r.Name )
            .HasColumnName( "name" )
            .HasMaxLength( 100 )
            .IsRequired();

        builder.Property( r => r.Description )
            .HasColumnName( "description" )
            .HasMaxLength( 150 )
            .IsRequired();

        builder.Property( r => r.CookTime )
            .HasColumnName( "cook_time" );

        builder.Property( r => r.PortionCount )
            .HasColumnName( "portion_count" );

        builder.Property( r => r.ImageName )
            .HasColumnName( "image_name" )
            .HasMaxLength( 100 )
            .IsRequired();

        builder.HasOne( r => r.Author )
            .WithMany( u => u.Recipes )
            .HasForeignKey( r => r.AuthorId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasMany( r => r.Tags )
            .WithMany( t => t.Recipes )
            .UsingEntity( "recipe_tags" );
    }
}
