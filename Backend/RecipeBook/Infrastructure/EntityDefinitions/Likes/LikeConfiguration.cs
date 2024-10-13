﻿using Domain.Entities;
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
            .HasForeignKey( f => f.UserId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( f => f.Recipe )
            .WithMany( r => r.Likes )
            .HasForeignKey( f => f.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
