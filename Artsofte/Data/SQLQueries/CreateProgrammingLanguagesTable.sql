USE [master]
USE [Artsofte.Data]
CREATE TABLE [dbo].[ProgrammingLanguages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_ProgrammingLanguages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY] 