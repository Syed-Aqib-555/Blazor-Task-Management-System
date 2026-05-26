# Blazor Task Management System - Application 08

This Blazor Server application connects a ToDo task interface to a local SQLite database. Tasks created, updated, completed, or deleted in the browser are persisted in `todo_tasks.db`.

## Features

- Professional dashboard with task summary metrics.
- Task create, update, complete, filter, and delete workflow.
- EF Core SQLite database connection through `TodoDbContext`.
- Local database initialization on application startup.
- Database output page at `/database` showing the same rows stored in SQLite.
- SQL schema script in `DatabaseScripts/create_todo_tasks.sql`.

## Run

```powershell
cd "C:\Users\Admin\source\repos\Blazor Task Management System\Blazor Task Management System"
dotnet run --launch-profile http
```

Open `http://localhost:5262`.

## Database

The connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "TodoDatabase": "Data Source=todo_tasks.db"
}
```

At startup, `TodoDatabaseInitializer` calls `EnsureCreatedAsync()` so the `Tasks` table is available before the UI loads.
