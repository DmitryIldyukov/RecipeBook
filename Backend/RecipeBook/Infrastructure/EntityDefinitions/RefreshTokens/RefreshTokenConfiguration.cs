using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityDefinitions.RefreshTokens;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure( EntityTypeBuilder<RefreshToken> builder )
    {
        builder.ToTable( "refresh_tokens" )
            .HasKey( r => r.Id );

        builder.Property( r => r.Id )
            .HasComment( "Id токена" )
            .HasColumnName( "refresh_token_id" )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property( t => t.UserId )
            .HasComment( "Id пользователя" )
            .HasColumnName( "user_id" )
            .IsRequired();

        builder.Property( t => t.Token )
            .HasComment( "Токен" )
            .HasColumnName( "token" )
            .IsRequired();

        builder.Property( t => t.ExpirationDate )
            .HasComment( "Дата истечения срока действия токена" )
            .HasColumnName( "expiration_date" )
            .IsRequired();

        builder.HasOne( t => t.User )
            .WithMany( r => r.RefreshTokens )
            .HasForeignKey( s => s.UserId )
            .OnDelete( DeleteBehavior.Cascade );
    }
}
