using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Infrastructure.Identity;
namespace UniversityAdmissions.Infrastructure.Persistence.Configurations;
public class AdmissionDecisionConfiguration : IEntityTypeConfiguration<AdmissionDecision>
{
    public void Configure(EntityTypeBuilder<AdmissionDecision> builder)
    {
        builder.ToTable("AdmissionDecisions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);
        builder.Property(x => x.Comments).HasMaxLength(1000);
        builder.HasOne(x => x.Admission).WithMany().HasForeignKey(x => x.AdmissionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.AdmissionId, x.DecisionDate });
        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}