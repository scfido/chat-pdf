using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace Letu.ChatPdf;

public class ChatPdfDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Host=localhost;Username=postgres;Password=123456;Database=ChatPdf";
        optionsBuilder.UseNpgsql(connectionString, o => o.UseVector());
    }

    public DbSet<PdfParagraph> PdfParagraphs => Set<PdfParagraph>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.Entity<PdfParagraph>()
            .Property(x => x.Content).HasMaxLength(1000);
        modelBuilder.Entity<PdfParagraph>()
            .Property(x=>x.Vector)
            .HasColumnType("vector(1024)");
    }
}