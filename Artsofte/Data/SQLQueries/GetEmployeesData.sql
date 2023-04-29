USE [Artsofte.Data]
SELECT 
i.Id, 
i.Name, 
i.Surname,
i.Age,
i.Gender,
i.DepartmentId,
i.ProgrammingLanguageId,
m.Name as deptName, 
m.Floor as deptFloor,
p.Name as ProgLangName
FROM dbo.Employees i 
INNER JOIN dbo.Departments m on m.Id = i.DepartmentId
INNER JOIN dbo.ProgrammingLanguages p on p.Id = i.ProgrammingLanguageId