CREATE PROCEDURE [dbo].[UpdateStudent]

	@Id INT, @SectionId INT, @YearResult INT

AS
BEGIN

	Update Student 
	Set
		SectionID = @SectionId,
		YearResult = @YearResult
	where
		Id = @Id

END