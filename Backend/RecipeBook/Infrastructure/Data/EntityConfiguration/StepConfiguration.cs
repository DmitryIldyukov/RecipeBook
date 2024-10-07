﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class StepConfiguration : IEntityTypeConfiguration<Step>
{
    public void Configure( EntityTypeBuilder<Step> builder )
    {
        builder.ToTable( "steps" )
            .HasKey( s => s.StepId );

        builder.Property( s => s.StepId )
            .HasComment( "Id шага" )
            .HasColumnName( "step_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( s => s.RecipeId )
            .HasComment( "Id рецепта" )
            .HasColumnName( "recipe_id" )
            .IsRequired();

        builder.Property( s => s.Description )
            .HasComment( "Описание шага" )
            .HasColumnName( "description" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.HasOne( s => s.Recipe )
            .WithMany( r => r.Steps )
            .HasForeignKey( s => s.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
