using Microsoft.EntityFrameworkCore;

namespace Blazor_Task_Management_System.Data;

public static class TodoDatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var factory = services.GetRequiredService<IDbContextFactory<TodoDbContext>>();
        await using var db = await factory.CreateDbContextAsync();

        await db.Database.EnsureCreatedAsync();
    }
}
