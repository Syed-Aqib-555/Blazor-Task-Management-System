using System.ComponentModel.DataAnnotations;

namespace Blazor_Task_Management_System.Models;

public sealed class TodoTaskFormModel
{
    [Required(ErrorMessage = "Task title is required.")]
    [StringLength(120, ErrorMessage = "Keep the task title under 120 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Keep notes under 500 characters.")]
    public string? Description { get; set; }

    [Required]
    public string Priority { get; set; } = "Medium";

    public DateTime? DueDate { get; set; }

    public static TodoTaskFormModel FromTask(TodoTask task) => new()
    {
        Title = task.Title,
        Description = task.Description,
        Priority = task.Priority,
        DueDate = task.DueDate
    };
}
