using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zetta.Infrastructure.Persistence.Preferences;

namespace Zetta.Infrastructure.Persistence.Configurations;

public sealed class AccountPreferenceConfiguration : IEntityTypeConfiguration<AccountPreference>
{
    public void Configure(EntityTypeBuilder<AccountPreference> builder)
    {
        builder.ToTable("account_preferences");

        builder.HasKey(p => p.AccountId);

        builder.Property(p => p.AccountId).HasColumnName("account_id");
        builder.Property(p => p.UserId).HasColumnName("user_id");
        builder.Property(p => p.Color).HasColumnName("color").HasMaxLength(7).IsRequired();
        builder.Property(p => p.Icon).HasColumnName("icon").HasMaxLength(50).IsRequired();
    }
}
