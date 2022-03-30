
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/25/2022 19:23:20
-- Generated from EDMX file: C:\1111111111111\MVC-Database-Disign-MusicWebsite\NewMusicWeb\NewMusicWeb\MusicDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [newMusic];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Song_Artist]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Songs] DROP CONSTRAINT [FK_Song_Artist];
GO
IF OBJECT_ID(N'[dbo].[FK_Bought_Song]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Boughts] DROP CONSTRAINT [FK_Bought_Song];
GO
IF OBJECT_ID(N'[dbo].[FK_Bought_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Boughts] DROP CONSTRAINT [FK_Bought_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Artists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Artists];
GO
IF OBJECT_ID(N'[dbo].[Boughts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Boughts];
GO
IF OBJECT_ID(N'[dbo].[Songs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Songs];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Artists'
CREATE TABLE [dbo].[Artists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Boughts'
CREATE TABLE [dbo].[Boughts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RatingValue] int  NOT NULL,
    [UserId] int  NOT NULL,
    [SongId] int  NOT NULL,
    [PurchaseDate] datetime  NULL
);
GO

-- Creating table 'Songs'
CREATE TABLE [dbo].[Songs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Price] float  NOT NULL,
    [ArtistId] int  NOT NULL,
    [OverallRating] float  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Money] float  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Artists'
ALTER TABLE [dbo].[Artists]
ADD CONSTRAINT [PK_Artists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Boughts'
ALTER TABLE [dbo].[Boughts]
ADD CONSTRAINT [PK_Boughts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Songs'
ALTER TABLE [dbo].[Songs]
ADD CONSTRAINT [PK_Songs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ArtistId] in table 'Songs'
ALTER TABLE [dbo].[Songs]
ADD CONSTRAINT [FK_Song_Artist]
    FOREIGN KEY ([ArtistId])
    REFERENCES [dbo].[Artists]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Song_Artist'
CREATE INDEX [IX_FK_Song_Artist]
ON [dbo].[Songs]
    ([ArtistId]);
GO

-- Creating foreign key on [SongId] in table 'Boughts'
ALTER TABLE [dbo].[Boughts]
ADD CONSTRAINT [FK_Bought_Song]
    FOREIGN KEY ([SongId])
    REFERENCES [dbo].[Songs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Bought_Song'
CREATE INDEX [IX_FK_Bought_Song]
ON [dbo].[Boughts]
    ([SongId]);
GO

-- Creating foreign key on [UserId] in table 'Boughts'
ALTER TABLE [dbo].[Boughts]
ADD CONSTRAINT [FK_Bought_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Bought_User'
CREATE INDEX [IX_FK_Bought_User]
ON [dbo].[Boughts]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------