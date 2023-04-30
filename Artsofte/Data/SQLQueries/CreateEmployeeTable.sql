USE [Artsofte.Data]
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Surname] [nvarchar](30) NOT NULL,
	[Age] [int] NOT NULL CHECK ([Age] >= 18 AND [Age] <= 65),
	[Gender] [nvarchar](30) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[ProgrammingLanguageId] [int] NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Departments_DepartmentId]
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_ProgrammingLanguages_ProgrammingLanguageId] FOREIGN KEY([ProgrammingLanguageId])
REFERENCES [dbo].[ProgrammingLanguages] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_ProgrammingLanguages_ProgrammingLanguageId]



