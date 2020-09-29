CREATE PROCEDURE [dbo].[DeleteStudent]
	
	@Id int

AS
BEGIN

	delete from Student 
	where Id = @Id

END