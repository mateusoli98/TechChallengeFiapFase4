USE master
GO

IF NOT EXISTS(SELECT * FROM master.sys.databases WHERE [name] = 
'ContactDB' )  
BEGIN 
  CREATE DATABASE ContactDB 
END

GO
USE ContactDB
GO

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

CREATE SEQUENCE [dbo].[ContactSequence] AS int START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
GO

CREATE TABLE [Contacts] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [AreaCode] smallint NOT NULL,
    [Phone] bigint NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [IsEnabled] bit NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526215132_Initial', N'8.0.8');
GO

COMMIT;
GO

