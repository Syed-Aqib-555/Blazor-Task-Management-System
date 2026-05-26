using Blazor_Task_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Task_Management_System.Data;

public sealed class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoTask> TodoTasks => Set<TodoTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var task = modelBuilder.Entity<TodoTask>();

        task.ToTable("Tasks");
        task.HasKey(item => item.Id);
        task.Property(item => item.Title).IsRequired().HasMaxLength(120);
        task.Property(item => item.Description).HasMaxLength(500);
        task.Property(item => item.Priority).IsRequired().HasMaxLength(20);
        task.Property(item => item.CreatedAt).IsRequired();
        task.HasIndex(item => item.IsCompleted);
        task.HasIndex(item => item.DueDate);

        task.HasData(
            new TodoTask
            {
                Id = 1,
                Title = "Review database manuals",
                Description = "Check the Week 13 database notes before final submission.",
                Priority = "High",
                DueDate = new DateTime(2026, 5, 27),
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 5, 26, 9, 0, 0)
            },
            new TodoTask
            {
                Id = 2,
                Title = "Capture client output screenshots",
                Description = "Add browser screenshots to the final assignment document.",
                Priority = "Medium",
                DueDate = new DateTime(2026, 5, 28),
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 5, 26, 9, 30, 0)
            },
            new TodoTask
            {
                Id = 3,
                Title = "Verify SQLite rows",
                Description = "Confirm create, update, and delete actions are saved locally.",
                Priority = "Low",
                DueDate = new DateTime(2026, 5, 29),
                IsCompleted = true,
                CreatedAt = new DateTime(2026, 5, 26, 10, 0, 0),
                CompletedAt = new DateTime(2026, 5, 26, 10, 30, 0)
            });
    }
}
