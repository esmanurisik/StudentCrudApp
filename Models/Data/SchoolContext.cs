using Microsoft.EntityFrameworkCore;
using StudentCrudApp.Models; // Modellerimizin bulunduğu klasörü içeri aktarıyoruz

namespace StudentCrudApp.Data;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }

    // Veritabanındaki tablomuzun adı Students olacak
    public DbSet<Student> Students => Set<Student>();
}