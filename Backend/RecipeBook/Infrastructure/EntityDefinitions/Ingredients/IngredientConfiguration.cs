using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Ingredients;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure( EntityTypeBuilder<Ingredient> builder )
    {
        builder.ToTable( "ingredients" )
            .HasKey( i => i.Id );

        builder.Property( i => i.Id )
            .HasColumnName( "ingredient_id" )
            .ValueGeneratedOnAdd();

        builder.Property( i => i.RecipeId )
            .HasColumnName( "recipe_id" );

        builder.Property( i => i.Title )
            .HasColumnName( "title" )
            .HasMaxLength( 40 )
            .IsRequired();

        builder.Property( i => i.Description )
            .HasColumnName( "description" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.HasOne( i => i.Recipe )
            .WithMany( r => r.Ingredients )
            .HasForeignKey( i => i.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
