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
        .HasOne<Author>()
        .WithMany(a => a.Books)
        .HasForeignKey(b => b.AuthorId);
    }
    //modelbuilder??
}
