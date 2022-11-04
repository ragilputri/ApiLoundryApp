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
GO

CREATE TABLE [Services] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Category] int NOT NULL,
    [Unit] int NOT NULL,
    [Price] float NOT NULL,
    [EstimationDuration] int NOT NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Email] nvarchar(100) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Password] nvarchar(200) NOT NULL,
    [Gender] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [Address] nvarchar(300) NOT NULL,
    [Role] int NOT NULL,
    [PhotoPath] nvarchar(200) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Email])
);
GO

CREATE TABLE [Packages] (
    [Id] uniqueidentifier NOT NULL,
    [ServiceId] uniqueidentifier NOT NULL,
    [Total] float NOT NULL,
    [Price] float NOT NULL,
    CONSTRAINT [PK_Packages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Packages_Services_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [Services] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [HeaderDeposits] (
    [Id] uniqueidentifier NOT NULL,
    [CustomerEmail] nvarchar(100) NOT NULL,
    [EmployeeEmail] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [EstimationAt] datetime2 NOT NULL,
    [CompletedAt] datetime2 NULL,
    CONSTRAINT [PK_HeaderDeposits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HeaderDeposits_Users_CustomerEmail] FOREIGN KEY ([CustomerEmail]) REFERENCES [Users] ([Email]),
    CONSTRAINT [FK_HeaderDeposits_Users_EmployeeEmail] FOREIGN KEY ([EmployeeEmail]) REFERENCES [Users] ([Email])
);
GO

CREATE TABLE [PackageTransactions] (
    [Id] uniqueidentifier NOT NULL,
    [UserEmail] nvarchar(100) NULL,
    [PackageId] uniqueidentifier NOT NULL,
    [Price] float NOT NULL,
    [AvailableUnit] float NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CompletedAt] datetime2 NULL,
    CONSTRAINT [PK_PackageTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PackageTransactions_Packages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [Packages] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PackageTransactions_Users_UserEmail] FOREIGN KEY ([UserEmail]) REFERENCES [Users] ([Email]) ON DELETE NO ACTION
);
GO

CREATE TABLE [DetailDeposits] (
    [Id] uniqueidentifier NOT NULL,
    [HeaderDepositId] uniqueidentifier NOT NULL,
    [ServiceId] uniqueidentifier NOT NULL,
    [PackageTransactionId] uniqueidentifier NULL,
    [Price] float NOT NULL,
    [Total] float NOT NULL,
    [CompletedAt] datetime2 NULL,
    CONSTRAINT [PK_DetailDeposits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DetailDeposits_HeaderDeposits_HeaderDepositId] FOREIGN KEY ([HeaderDepositId]) REFERENCES [HeaderDeposits] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DetailDeposits_PackageTransactions_PackageTransactionId] FOREIGN KEY ([PackageTransactionId]) REFERENCES [PackageTransactions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_DetailDeposits_Services_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [Services] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_DetailDeposits_HeaderDepositId] ON [DetailDeposits] ([HeaderDepositId]);
GO

CREATE INDEX [IX_DetailDeposits_PackageTransactionId] ON [DetailDeposits] ([PackageTransactionId]);
GO

CREATE INDEX [IX_DetailDeposits_ServiceId] ON [DetailDeposits] ([ServiceId]);
GO

CREATE INDEX [IX_HeaderDeposits_CustomerEmail] ON [HeaderDeposits] ([CustomerEmail]);
GO

CREATE INDEX [IX_HeaderDeposits_EmployeeEmail] ON [HeaderDeposits] ([EmployeeEmail]);
GO

CREATE INDEX [IX_Packages_ServiceId] ON [Packages] ([ServiceId]);
GO

CREATE INDEX [IX_PackageTransactions_PackageId] ON [PackageTransactions] ([PackageId]);
GO

CREATE INDEX [IX_PackageTransactions_UserEmail] ON [PackageTransactions] ([UserEmail]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211027203436_Init', N'5.0.11');
GO

COMMIT;
GO


