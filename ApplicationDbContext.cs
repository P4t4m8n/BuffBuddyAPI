using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

public class ApplicationDbContext : DbContext
{
  public DbSet<Exercise> Exercises { get; set; }
  public DbSet<ExerciseMuscle> ExerciseMuscles { get; set; }
  public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Exercise>()
  .Property(e => e.Id)
  .HasDefaultValueSql("NEWID()");


    modelBuilder.Entity<ExerciseMuscle>()
    .Property(e => e.CreateAt)
    .HasDefaultValueSql("GETUTCDATE()");


  }

}