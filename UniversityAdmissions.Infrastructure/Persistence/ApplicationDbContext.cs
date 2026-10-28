using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Infrastructure.Identity;
namespace UniversityAdmissions.Infrastructure.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<Admission> Admissions => Set<Admission>();
    public DbSet<AdmissionDocument> AdmissionDocuments => Set<AdmissionDocument>();
    public DbSet<Gender> Genders => Set<Gender>();
    public DbSet<IdentificationType> IdentificationTypes => Set<IdentificationType>();
    public DbSet<AdmissionStatus> AdmissionStatuses => Set<AdmissionStatus>();
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
    public DbSet<AdmissionDecision> AdmissionDecisions => Set<AdmissionDecision>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<Gender>().HasData(
            new Gender { Id = 1, Name = "Masculino" },
            new Gender { Id = 2, Name = "Femenino" }
        );
        modelBuilder.Entity<IdentificationType>().HasData(
            new IdentificationType { Id = 1, Name = "Cédula" },
            new IdentificationType { Id = 2, Name = "Pasaporte" }
        );
        modelBuilder.Entity<AdmissionStatus>().HasData(
            new AdmissionStatus { Id = 1, Name = "Pendiente" },
            new AdmissionStatus { Id = 2, Name = "En revisión" },
            new AdmissionStatus { Id = 3, Name = "Aprobada" },
            new AdmissionStatus { Id = 4, Name = "Rechazada" }
        );
        modelBuilder.Entity<DocumentType>().HasData(
            new DocumentType { Id = 1, Name = "Fotografía" },
            new DocumentType { Id = 2, Name = "Identificación" },
            new DocumentType { Id = 3, Name = "Acta de nacimiento" },
            new DocumentType { Id = 4, Name = "Certificado de estudios" },
            new DocumentType { Id = 5, Name = "Otro documento" }
        );
    }
}