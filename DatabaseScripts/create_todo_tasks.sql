CREATE TABLE IF NOT EXISTS Tasks (
    Id INTEGER NOT NULL CONSTRAINT PK_Tasks PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NULL,
    Priority TEXT NOT NULL,
    DueDate TEXT NULL,
    IsCompleted INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    CompletedAt TEXT NULL
);

CREATE INDEX IF NOT EXISTS IX_Tasks_IsCompleted ON Tasks (IsCompleted);
CREATE INDEX IF NOT EXISTS IX_Tasks_DueDate ON Tasks (DueDate);

INSERT INTO Tasks (Title, Description, Priority, DueDate, IsCompleted, CreatedAt, UpdatedAt, CompletedAt)
SELECT 'Review database manuals',
       'Check the Week 13 database notes before final submission.',
       'High',
       '2026-05-27 00:00:00',
       0,
       '2026-05-26 09:00:00',
       NULL,
       NULL
WHERE NOT EXISTS (SELECT 1 FROM Tasks);
