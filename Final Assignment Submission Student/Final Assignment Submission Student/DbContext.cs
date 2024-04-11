using System.Data.Entity;

public class StudentDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
}
