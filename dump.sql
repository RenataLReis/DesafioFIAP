IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Alunos] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [DataNascimento] datetime2 NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [CPF] nvarchar(450) NOT NULL,
    [SenhaHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Alunos] PRIMARY KEY ([Id])
);

CREATE TABLE [Turmas] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Descricao] nvarchar(max) NULL,
    CONSTRAINT [PK_Turmas] PRIMARY KEY ([Id])
);

CREATE TABLE [Matriculas] (
    [Id] int NOT NULL IDENTITY,
    [AlunoId] int NOT NULL,
    [TurmaId] int NOT NULL,
    [Numero] nvarchar(max) NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NULL,
    [Ativa] bit NOT NULL,
    CONSTRAINT [PK_Matriculas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Matriculas_Alunos_AlunoId] FOREIGN KEY ([AlunoId]) REFERENCES [Alunos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Matriculas_Turmas_TurmaId] FOREIGN KEY ([TurmaId]) REFERENCES [Turmas] ([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Alunos_CPF] ON [Alunos] ([CPF]);

CREATE UNIQUE INDEX [IX_Alunos_Email] ON [Alunos] ([Email]);

CREATE UNIQUE INDEX [IX_Matriculas_AlunoId_TurmaId] ON [Matriculas] ([AlunoId], [TurmaId]);

CREATE INDEX [IX_Matriculas_TurmaId] ON [Matriculas] ([TurmaId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250719222043_InitialCreate', N'9.0.7');

CREATE TABLE [Administradores] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(450) NOT NULL,
    [SenhaHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Administradores] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Administradores_Email] ON [Administradores] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250721124641_AddAdministrador', N'9.0.7');

COMMIT;
GO

