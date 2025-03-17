using Microsoft.EntityFrameworkCore;

namespace Library.Models;

public class LibraryContext : DbContext
{

    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {

    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Book>()
        .HasIndex(b => b.Title)
        .HasDatabaseName("idx_books_title");

        modelBuilder.Entity<Book>()
        .HasIndex(b => b.AuthorId)
        .HasDatabaseName("idx_books_author");

        modelBuilder.Entity<Book>()
        .HasIndex(b => new { b.Title, b.Id })
        .HasDatabaseName("idx_books_pagination");
    }
    //modelbuilder??
}
