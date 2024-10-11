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
            .HasComment( "Id шага" )
            .HasColumnName( "step_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( s => s.Description )
            .HasComment( "Описание шага" )
            .HasColumnName( "description" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.HasOne( s => s.Recipe )
            .WithMany( r => r.Steps )
            .HasForeignKey( "recipe_id" )
            .IsRequired()
            .OnDelete( DeleteBehavior.Cascade );
    }
}
