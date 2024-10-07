using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure( EntityTypeBuilder<User> builder )
    {
        builder.ToTable( "users" )
            .HasKey( u => u.UserId );

        builder.Property( u => u.UserId )
            .HasComment( "Id пользователя" )
            .HasColumnName( "user_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( u => u.Name )
            .HasComment( "Имя пользователя" )
            .HasColumnName( "name" )
            .HasMaxLength( 50 )
            .IsRequired();

        builder.Property( u => u.Login )
            .HasComment( "Логин" )
            .HasColumnName( "login" )
            .HasMaxLength( 30 )
            .IsRequired();

        builder.Property( u => u.Email )
            .HasComment( "Электронная почта" )
            .HasColumnName( "email" )
            .HasMaxLength( 254 )
            .IsRequired();

        builder.Property( u => u.Password )
            .HasComment( "Пароль" )
            .HasColumnName( "password" )
            .HasMaxLength( 255 )
            .IsRequired();

        builder.Property( u => u.Information )
            .HasComment( "Информация о себе" )
            .HasColumnName( "information" )
            .HasMaxLength( 255 )
            .IsRequired();
    }
}
