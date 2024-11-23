using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Tags;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure( EntityTypeBuilder<Tag> builder )
    {
        builder.ToTable( "tags" )
            .HasKey( t => t.Id );

        builder.Property( t => t.Id )
            .HasColumnName( "tag_id" )
            .ValueGeneratedOnAdd();

        builder.Property( t => t.Name )
            .HasColumnName( "name" )
            .HasMaxLength( 20 )
            .IsRequired();
    }
}
