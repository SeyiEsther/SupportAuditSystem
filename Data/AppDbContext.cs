using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Models;

namespace SupportAuditSystem.Data
{
    // Entity Framework owns the schema through migrations — nothing is created by
    // hand. Every free-text / notes column is nvarchar(max) (this family of apps
    // has a history of truncation crashes); bounded key columns stay well under
    // the 1700-byte nonclustered index limit.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Area> Areas => Set<Area>();
        public DbSet<Shift> Shifts => Set<Shift>();
        public DbSet<TaskList> TaskLists => Set<TaskList>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<TaskCheckpoint> TaskCheckpoints => Set<TaskCheckpoint>();
        public DbSet<ChecklistSubmission> ChecklistSubmissions => Set<ChecklistSubmission>();
        public DbSet<TaskResponse> TaskResponses => Set<TaskResponse>();
        public DbSet<CheckpointResponse> CheckpointResponses => Set<CheckpointResponse>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Department>(e =>
            {
                e.Property(d => d.Name).HasMaxLength(200);
                e.HasIndex(d => d.Name).IsUnique();
                e.HasMany(d => d.Areas).WithOne(a => a.Department!)
                 .HasForeignKey(a => a.DepartmentId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(d => d.Shifts).WithOne(s => s.Department!)
                 .HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.Cascade);
            });

            mb.Entity<Area>(e =>
            {
                e.Property(a => a.Name).HasMaxLength(200);
                e.Property(a => a.DefaultLocation).HasColumnType("nvarchar(max)");
                // UNIQUE INDEX #1 — an area name is unique within its department,
                // so admin cannot create two "Consumables" under Stores.
                e.HasIndex(a => new { a.DepartmentId, a.Name }).IsUnique();
            });

            mb.Entity<Shift>(e =>
            {
                e.Property(s => s.Name).HasMaxLength(200);
                // UNIQUE INDEX #2 — a shift name is unique within its department.
                e.HasIndex(s => new { s.DepartmentId, s.Name }).IsUnique();
            });

            mb.Entity<TaskList>(e =>
            {
                e.Property(t => t.HealthRepsReminder).HasColumnType("nvarchar(max)");
                e.Property(t => t.CreatedBy).HasMaxLength(256);
                e.HasOne(t => t.Area).WithMany()
                 .HasForeignKey(t => t.AreaId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(t => t.Shift).WithMany()
                 .HasForeignKey(t => t.ShiftId).OnDelete(DeleteBehavior.Restrict);
                e.HasMany(t => t.Items).WithOne(i => i.TaskList!)
                 .HasForeignKey(i => i.TaskListId).OnDelete(DeleteBehavior.Cascade);
                // One current version per Area+Shift is enforced in code (the
                // version bump clears the previous current flag); the lookup is
                // indexed but not unique so a concurrent edit can't hard-fail.
                e.HasIndex(t => new { t.AreaId, t.ShiftId, t.IsCurrent });
            });

            mb.Entity<TaskItem>(e =>
            {
                e.Property(i => i.Text).HasColumnType("nvarchar(max)");
                e.HasMany(i => i.Checkpoints).WithOne(c => c.TaskItem!)
                 .HasForeignKey(c => c.TaskItemId).OnDelete(DeleteBehavior.Cascade);
            });

            mb.Entity<TaskCheckpoint>(e =>
            {
                e.Property(c => c.Label).HasMaxLength(120);
            });

            mb.Entity<ChecklistSubmission>(e =>
            {
                e.Property(s => s.AuditorNames).HasColumnType("nvarchar(max)");
                e.Property(s => s.Location).HasColumnType("nvarchar(max)");
                e.Property(s => s.CreatedBy).HasMaxLength(256);
                e.Property(s => s.LastEditedBy).HasMaxLength(256);
                e.Property(s => s.CompletedBy).HasMaxLength(256);
                e.HasOne(s => s.Area).WithMany()
                 .HasForeignKey(s => s.AreaId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(s => s.Shift).WithMany()
                 .HasForeignKey(s => s.ShiftId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(s => s.TaskList).WithMany()
                 .HasForeignKey(s => s.TaskListId).OnDelete(DeleteBehavior.Restrict);
                e.HasMany(s => s.Responses).WithOne(r => r.ChecklistSubmission!)
                 .HasForeignKey(r => r.ChecklistSubmissionId).OnDelete(DeleteBehavior.Cascade);
                // Natural-key lookup (date + area + shift). Non-unique so a
                // concurrent insert can't hard-fail; continuity is resolved in
                // code via find-or-create.
                e.HasIndex(s => new { s.ChecklistDate, s.AreaId, s.ShiftId });
            });

            mb.Entity<TaskResponse>(e =>
            {
                e.Property(r => r.Status).HasMaxLength(20);
                e.Property(r => r.Notes).HasColumnType("nvarchar(max)");
                e.HasOne(r => r.TaskItem).WithMany()
                 .HasForeignKey(r => r.TaskItemId).OnDelete(DeleteBehavior.Restrict);
                e.HasMany(r => r.CheckpointResponses).WithOne(c => c.TaskResponse!)
                 .HasForeignKey(c => c.TaskResponseId).OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(r => new { r.ChecklistSubmissionId, r.TaskItemId });
            });

            mb.Entity<CheckpointResponse>(e =>
            {
                e.HasOne(c => c.TaskCheckpoint).WithMany()
                 .HasForeignKey(c => c.TaskCheckpointId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
