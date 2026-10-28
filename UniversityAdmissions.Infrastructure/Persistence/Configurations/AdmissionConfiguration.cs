using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Infrastructure.Persistence.Configurations;
public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
{
    public void Configure(EntityTypeBuilder<Admission> builder)
    {
        builder.ToTable("Admissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ApplicationDate).IsRequired();
        builder.Property(x => x.AcceptTerms).IsRequired();
        builder.HasOne(x => x.Applicant)
            .WithMany(x => x.Admissions)
            .HasForeignKey(x => x.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.AdmissionStatus)
            .WithMany(x => x.Admissions)
            .HasForeignKey(x => x.AdmissionStatusId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.ApplicantId);
    }
}