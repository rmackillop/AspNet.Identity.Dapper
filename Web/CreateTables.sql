

CREATE TABLE [dbo].[Member] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.Member] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [MemberNameIndex]
    ON [dbo].[Member]([UserName] ASC);



CREATE TABLE [dbo].[MemberClaim] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [MemberId]   INT            NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.MemberClaim] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.MemberClaim_dbo.Member_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IX_MemberId]
    ON [dbo].[MemberClaim]([MemberId] ASC);



CREATE TABLE [dbo].[MemberLogin] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [MemberId]      INT            NOT NULL,
    CONSTRAINT [PK_dbo.MemberLogin] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [MemberId] ASC),
    CONSTRAINT [FK_dbo.MemberLogin_dbo.Member_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IX_MemberId]
    ON [dbo].[MemberLogin]([MemberId] ASC);



CREATE TABLE [dbo].[Role] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[Role]([Name] ASC);



CREATE TABLE [dbo].[MemberRole] (
    [MemberId] INT NOT NULL,
    [RoleId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.MemberRoles] PRIMARY KEY CLUSTERED ([MemberId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.MemberRoles_dbo.Member_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.MemberRoles_dbo.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IX_MemberId]
    ON [dbo].[MemberRole]([MemberId] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[MemberRole]([RoleId] ASC);

