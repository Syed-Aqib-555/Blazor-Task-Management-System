using Blazor_Task_Management_System.Data;
using Blazor_Task_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Task_Management_System.Services;

public sealed class TodoTaskService(IDbContextFactory<TodoDbContext> dbContextFactory)
{
    public async Task<List<TodoTask>> GetTasksAsync(string filter = "All")
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var query = db.TodoTasks.AsNoTracking();

        query = filter switch
        {
            "Open" => query.Where(task => !task.IsCompleted),
            "Completed" => query.Where(task => task.IsCompleted),
            "Overdue" => query.Where(task => !task.IsCompleted
                && task.DueDate.HasValue
                && task.DueDate.Value.Date < DateTime.Today),
            _ => query
        };

        return await query
            .OrderBy(task => task.IsCompleted)
            .ThenBy(task => task.DueDate ?? DateTime.MaxValue)
            .ThenByDescending(task => task.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TodoTask>> GetRecentTasksAsync(int count)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        return await db.TodoTasks
            .AsNoTracking()
            .OrderByDescending(task => task.UpdatedAt ?? task.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<TodoTask?> GetTaskAsync(int id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.TodoTasks.AsNoTracking().FirstOrDefaultAsync(task => task.Id == id);
    }

    public async Task<TodoTask> CreateTaskAsync(TodoTaskFormModel model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var task = new TodoTask
        {
            Title = model.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            Priority = model.Priority,
            DueDate = model.DueDate?.Date,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        db.TodoTasks.Add(task);
        await db.SaveChangesAsync();

        return task;
    }

    public async Task<bool> UpdateTaskAsync(int id, TodoTaskFormModel model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var task = await db.TodoTasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }

        task.Title = model.Title.Trim();
        task.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        task.Priority = model.Priority;
        task.DueDate = model.DueDate?.Date;
        task.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleCompletionAsync(int id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var task = await db.TodoTasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }

        task.IsCompleted = !task.IsCompleted;
        task.CompletedAt = task.IsCompleted ? DateTime.Now : null;
        task.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var task = await db.TodoTasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }

        db.TodoTasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<TodoDashboardStats> GetStatsAsync()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var today = DateTime.Today;

        var total = await db.TodoTasks.CountAsync();
        var open = await db.TodoTasks.CountAsync(task => !task.IsCompleted);
        var completed = await db.TodoTasks.CountAsync(task => task.IsCompleted);
        var overdue = await db.TodoTasks.CountAsync(task =>
            !task.IsCompleted && task.DueDate.HasValue && task.DueDate.Value.Date < today);
        var dueToday = await db.TodoTasks.CountAsync(task =>
            !task.IsCompleted && task.DueDate.HasValue && task.DueDate.Value.Date == today);

        return new TodoDashboardStats(total, open, completed, overdue, dueToday);
    }
}
