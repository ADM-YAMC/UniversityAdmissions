using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Infrastructure.Persistence.Configurations;
public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        builder.ToTable("Applicants");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IdentificationNumber).IsRequired().HasMaxLength(30);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(16);
        builder.Property(x => x.DateOfBirth).HasColumnType("date");
        builder.HasIndex(x => new { x.IdentificationTypeId, x.IdentificationNumber }).IsUnique();
        builder.HasOne(x => x.Gender).WithMany(x => x.Applicants).HasForeignKey(x => x.GenderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.IdentificationType).WithMany(x => x.Applicants).HasForeignKey(x => x.IdentificationTypeId).OnDelete(DeleteBehavior.Restrict);
    }
}