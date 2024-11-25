using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Steps;

public class StepConfiguration : IEntityTypeConfiguration<Step>
{
    public void Configure( EntityTypeBuilder<Step> builder )
    {
        builder.ToTable( "steps" )
            .HasKey( s => s.Id );

        builder.Property( s => s.Id )
            .HasColumnName( "step_id" )
            .ValueGeneratedOnAdd();

        builder.Property( s => s.RecipeId )
            .HasColumnName( "recipe_id" );

        builder.Property( s => s.Description )
            .HasColumnName( "description" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.HasOne( s => s.Recipe )
            .WithMany( r => r.Steps )
            .HasForeignKey( s => s.RecipeId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
