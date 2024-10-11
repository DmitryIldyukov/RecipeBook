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
            .HasComment( "Id рецепта" )
            .HasColumnName( "recipe_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( r => r.AuthorId )
            .HasComment( "Id автора" )
            .HasColumnName( "author_id" )
            .IsRequired();

        builder.Property( r => r.Name )
            .HasComment( "Название рецепта" )
            .HasColumnName( "name" )
            .HasMaxLength( 100 )
            .IsRequired();

        builder.Property( r => r.Description )
            .HasComment( "Описание" )
            .HasColumnName( "description" )
            .HasMaxLength( 150 )
            .IsRequired();

        builder.Property( r => r.CookTime )
            .HasComment( "Время готовки в минутах" )
            .HasColumnName( "cook_time" )
            .IsRequired();

        builder.Property( r => r.PortionCount )
            .HasComment( "Порций в блюде" )
            .HasColumnName( "portion_count" )
            .IsRequired();

        builder.Property( r => r.ImageName )
            .HasComment( "Название фото блюда" )
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
