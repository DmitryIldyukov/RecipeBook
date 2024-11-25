using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure( EntityTypeBuilder<User> builder )
    {
        builder.ToTable( "users" )
            .HasKey( u => u.Id );

        builder.Property( u => u.Id )
            .HasColumnName( "user_id" )
            .ValueGeneratedOnAdd();

        builder.Property( u => u.Name )
            .HasColumnName( "name" )
            .HasMaxLength( 50 )
            .IsRequired();

        builder.Property( u => u.Login )
            .HasColumnName( "login" )
            .HasMaxLength( 30 )
            .IsRequired();

        builder.Property( u => u.Email )
            .HasColumnName( "email" )
            .HasMaxLength( 254 )
            .IsRequired();

        builder.Property( u => u.Password )
            .HasColumnName( "password" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.Property( u => u.Information )
            .HasColumnName( "information" )
            .HasMaxLength( 255 )
            .IsRequired();
    }
}
