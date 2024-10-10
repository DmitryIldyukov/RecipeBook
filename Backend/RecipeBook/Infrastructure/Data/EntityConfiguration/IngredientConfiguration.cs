using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure( EntityTypeBuilder<Ingredient> builder )
    {
        builder.ToTable( "ingredients" )
            .HasKey( i => i.Id );

        builder.Property( i => i.Id )
            .HasComment( "Id ингредиента" )
            .HasColumnName( "ingredient_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( i => i.RecipeId )
            .HasComment( "Id рецепта" )
            .HasColumnName( "recipe_id" )
            .IsRequired();

        builder.Property( i => i.Title )
            .HasComment( "Заголовок для игредиентов" )
            .HasColumnName( "title" )
            .HasMaxLength( 40 )
            .IsRequired();

        builder.Property( i => i.Description )
            .HasComment( "Список продуктов" )
            .HasColumnName( "description" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.HasOne( i => i.Recipe )
            .WithMany( r => r.Ingredients )
            .HasForeignKey( i => i.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
