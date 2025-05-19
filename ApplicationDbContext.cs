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
  public DbSet<CoreSet> CoreSets { get; set; }
  public DbSet<UserSet> UserSets { get; set; }
  public ApplicationDbContext(DbContextOptions options) : base(options) { }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Exercise>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    modelBuilder.Entity<Program>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    // Configure cascade delete for Program -> ProgramExercise
    modelBuilder.Entity<Program>()
        .HasMany(p => p.ProgramExercises)
        .WithOne(pe => pe.Program)
        .HasForeignKey(pe => pe.ProgramId)
        .OnDelete(DeleteBehavior.Cascade);

    // Configure cascade delete for ProgramExercise -> Set
    modelBuilder.Entity<ProgramExercise>()
        .HasMany(pe => pe.CoreSets)
        .WithOne(s => s.ProgramExercise)
        .HasForeignKey(s => s.ProgramExerciseId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<UserSet>()
    .HasOne(us => us.ProgramExercise)
    .WithMany()
    .HasForeignKey(us => us.ProgramExerciseId)
    .OnDelete(DeleteBehavior.NoAction);
    modelBuilder.Entity<UserSet>()
       .HasOne(us => us.CoreSet)
       .WithMany()
       .HasForeignKey(us => us.CoreSetId)
       .OnDelete(DeleteBehavior.NoAction);



    modelBuilder.Entity<ProgramExercise>()
      .Property(e => e.Id)
      .HasDefaultValueSql("NEWID()");

    modelBuilder.Entity<CoreSet>()
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