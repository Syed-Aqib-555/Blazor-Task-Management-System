namespace Blazor_Task_Management_System.Models;

public sealed record TodoDashboardStats(
    int TotalTasks,
    int OpenTasks,
    int CompletedTasks,
    int OverdueTasks,
    int DueTodayTasks);
