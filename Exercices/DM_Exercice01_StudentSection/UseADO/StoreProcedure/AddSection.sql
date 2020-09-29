CREATE PROCEDURE [dbo].[AddSection]

	@Id int, @sectionName varchar(50)

AS
BEGIN

	insert into Section 
		(Id, SectionName) 
	values 
		(@Id, @sectionName)

END