using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ogma.Infrastructure.Persistence.Partners.Models;

namespace Ogma.Infrastructure.Persistence.Partners.Configurations;

internal class PartnerConfiguration : IEntityTypeConfiguration<Partner>
{
    public void Configure(EntityTypeBuilder<Partner> builder)
    {
        builder.OwnsOne(p => p.HQAddress, ab =>
        {
            ab.ToJson();
        });

        builder.OwnsMany(p => p.BankAccounts, pba =>
        {
            pba.WithOwner(pba => pba.Partner);

            pba.ToTable("partner_bank_accounts");

            pba.OwnsOne(pba => pba.BankAccount, bank =>
            {
                bank.Property(b => b.Bank).HasColumnName("bank");
                bank.Property(b => b.Iban).HasColumnName("iban");
                bank.Property(b => b.Currency).HasColumnName("currency");
                bank.Property(b => b.Bic).HasColumnName("bic");
            });
        });

        builder.HasMany(p => p.Roles)
            .WithMany()
            .UsingEntity(
                l => l.HasOne(typeof(PartnerRoleType))
                        .WithMany()
                        .HasForeignKey("roles_id")
                        .OnDelete(DeleteBehavior.Restrict), 

                r => r.HasOne(typeof(Partner))
                        .WithMany()
                        .HasForeignKey("partner_id")
                        .OnDelete(DeleteBehavior.Cascade), 

                j =>
                {
                    j.ToTable("partner_roles");
                    j.HasKey("partner_id", "roles_id");
                });
    }
}
