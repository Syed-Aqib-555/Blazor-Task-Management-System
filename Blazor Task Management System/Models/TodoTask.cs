using System.ComponentModel.DataAnnotations;

namespace Blazor_Task_Management_System.Models;

public class TodoTask
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required, StringLength(20)]
    public string Priority { get; set; } = "Medium";

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}
