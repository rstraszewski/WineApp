CREATE TABLE [dbo].[Users] (
    [Id]            INT            PRIMARY KEY		IDENTITY (1, 1) NOT NULL,
    [Username]      NVARCHAR (128) NOT NULL,
    [Email]         NVARCHAR (128) NOT NULL,
    [CredentialsId] INT            NOT NULL,
    CONSTRAINT [FK_dbo.Users_dbo.Credentials_CredentialsId] FOREIGN KEY ([CredentialsId]) REFERENCES [dbo].[Credentials] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CredentialsId]
    ON [dbo].[Users]([CredentialsId] ASC);

