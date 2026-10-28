using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Infrastructure.Persistence.Configurations;
public class AdmissionDocumentConfiguration : IEntityTypeConfiguration<AdmissionDocument>
{
    public void Configure(EntityTypeBuilder<AdmissionDocument> builder)
    {
        builder.ToTable("AdmissionDocuments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.StoredFileName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.StoredFileName).IsUnique();
        builder.HasOne(x => x.Admission)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.AdmissionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.DocumentType)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.DocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}