using Microsoft.EntityFrameworkCore;
namespace BuffBuddyAPI;
public class ApplicationDbContext : DbContext
{
  public DbSet<Exercise> Exercises { get; set; }
  public DbSet<ExerciseMuscle> ExerciseMuscles { get; set; }
  public DbSet<ExerciseType> ExerciseTypes { get; set; }
  public DbSet<ExerciseEquipment> ExerciseEquipments { get; set; }
  public DbSet<Program> Programs { get; set; }
  public DbSet<ProgramExercise> ProgramExercises { get; set; }
  public DbSet<Set> Sets { get; set; }
  public ApplicationDbContext(DbContextOptions options) : base(options) { }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Exercise>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    modelBuilder.Entity<Program>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    modelBuilder.Entity<ProgramExercise>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    modelBuilder.Entity<Set>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    modelBuilder.Entity<ExerciseMuscle>()
      .Property(e => e.CreateAt)
      .HasDefaultValueSql("GETUTCDATE()");

    modelBuilder.Entity<ExerciseType>()
      .Property(e => e.CreateAt)
      .HasDefaultValueSql("GETUTCDATE()");

    modelBuilder.Entity<ExerciseEquipment>()
      .Property(e => e.CreateAt)
      .HasDefaultValueSql("GETUTCDATE()");
  }
}