using Blazor_Task_Management_System.Components;
using Blazor_Task_Management_System.Data;
using Blazor_Task_Management_System.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var configuredConnectionString = builder.Configuration.GetConnectionString("TodoDatabase")
    ?? "Data Source=todo_tasks.db";
var connectionBuilder = new SqliteConnectionStringBuilder(configuredConnectionString);

if (!Path.IsPathRooted(connectionBuilder.DataSource))
{
    connectionBuilder.DataSource = Path.Combine(
        builder.Environment.ContentRootPath,
        connectionBuilder.DataSource);
}

var databaseInfo = new DatabaseInfo(connectionBuilder.DataSource, connectionBuilder.ToString());

builder.Services.AddSingleton(databaseInfo);
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
    options.UseSqlite(databaseInfo.ConnectionString));
builder.Services.AddScoped<TodoTaskService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await TodoDatabaseInitializer.InitializeAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
