cd IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [Departments] (
        [DepartmentId] int NOT NULL IDENTITY,
        [DepartmentName] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Departments] PRIMARY KEY ([DepartmentId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [Designations] (
        [DesignationId] int NOT NULL IDENTITY,
        [DesignationName] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Designations] PRIMARY KEY ([DesignationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [Regions] (
        [RegionId] int NOT NULL IDENTITY,
        [RegionName] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Regions] PRIMARY KEY ([RegionId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [Roles] (
        [RoleId] int NOT NULL IDENTITY,
        [RoleName] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [Employees] (
        [EmployeeId] int NOT NULL IDENTITY,
        [Username] nvarchar(50) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [EmployeeCode] nvarchar(50) NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [EmailId] nvarchar(100) NOT NULL,
        [DateOfJoining] datetime2 NOT NULL,
        [RegionId] int NOT NULL,
        [DepartmentId] int NOT NULL,
        [DesignationId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeId]),
        CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([DepartmentId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Employees_Designations_DesignationId] FOREIGN KEY ([DesignationId]) REFERENCES [Designations] ([DesignationId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Employees_Regions_RegionId] FOREIGN KEY ([RegionId]) REFERENCES [Regions] ([RegionId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Employees_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [Attendances] (
        [AttendanceId] int NOT NULL IDENTITY,
        [EmployeeId] int NOT NULL,
        [Date] datetime2 NOT NULL,
        [CheckInTime] datetime2 NOT NULL,
        [CheckOutTime] datetime2 NULL,
        [Status] nvarchar(50) NOT NULL,
        [LocationAddress] nvarchar(max) NOT NULL,
        [Latitude] float NOT NULL,
        [Longitude] float NOT NULL,
        [PhotoPath] nvarchar(max) NOT NULL,
        [VerifiedByHRId] int NULL,
        CONSTRAINT [PK_Attendances] PRIMARY KEY ([AttendanceId]),
        CONSTRAINT [FK_Attendances_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([EmployeeId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE TABLE [RefreshTokens] (
        [Id] int NOT NULL IDENTITY,
        [TokenId] nvarchar(450) NOT NULL,
        [TokenHash] nvarchar(max) NOT NULL,
        [EmployeeId] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        [IsRevoked] bit NOT NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RefreshTokens_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([EmployeeId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Attendances_EmployeeId_Date] ON [Attendances] ([EmployeeId], [Date]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE INDEX [IX_Employees_DepartmentId] ON [Employees] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE INDEX [IX_Employees_DesignationId] ON [Employees] ([DesignationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Employees_EmailId] ON [Employees] ([EmailId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Employees_EmployeeCode] ON [Employees] ([EmployeeCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE INDEX [IX_Employees_RegionId] ON [Employees] ([RegionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE INDEX [IX_Employees_RoleId] ON [Employees] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Employees_Username] ON [Employees] ([Username]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE INDEX [IX_RefreshTokens_EmployeeId] ON [RefreshTokens] ([EmployeeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RefreshTokens_TokenId] ON [RefreshTokens] ([TokenId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251213054546_InitialDomain'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251213054546_InitialDomain', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251218052124_FixRefreshTokenDateColumns'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RefreshTokens]') AND [c].[name] = N'IsRevoked');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [RefreshTokens] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [RefreshTokens] ADD DEFAULT CAST(0 AS bit) FOR [IsRevoked];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251218052124_FixRefreshTokenDateColumns'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RefreshTokens]') AND [c].[name] = N'ExpiresAt');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [RefreshTokens] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [RefreshTokens] ALTER COLUMN [ExpiresAt] datetime2(7) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251218052124_FixRefreshTokenDateColumns'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RefreshTokens]') AND [c].[name] = N'CreatedAt');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [RefreshTokens] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [RefreshTokens] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251218052124_FixRefreshTokenDateColumns'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251218052124_FixRefreshTokenDateColumns', N'8.0.0');
END;
GO

COMMIT;
GO

