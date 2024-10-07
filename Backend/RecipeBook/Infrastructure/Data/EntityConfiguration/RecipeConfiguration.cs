﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure( EntityTypeBuilder<Recipe> builder )
    {
        builder.ToTable( "recipes" )
            .HasKey( r => r.RecipeId );

        builder.Property( r => r.RecipeId )
            .HasComment( "Id рецепта" )
            .HasColumnName( "recipe_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( r => r.UserId )
            .HasComment( "Id пользователя" )
            .HasColumnName( "user_id" )
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

        builder.HasOne( r => r.User )
            .WithMany( u => u.Recipes )
            .HasForeignKey( r => r.UserId )
            .OnDelete( DeleteBehavior.Cascade );

        builder.HasMany( r => r.Tags )
            .WithMany( t => t.Recipes )
            .UsingEntity( "recipe_tags" );
    }
}
