CREATE TABLE [dbo].[Student]
(
	[Id] INT NOT NULL IDENTITY , 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [BirthDate] DATETIME2 NOT NULL, 
    [YearResult] INT NOT NULL, 
    [SectionID] INT NOT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_Student] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Student_Section] FOREIGN KEY (SectionID) REFERENCES Section(Id), 
    CONSTRAINT [CK_Student_YearResult_0_20] CHECK (YearResult between 0 and 20), 
    CONSTRAINT [CK_Student_BirthDate_1930] CHECK (BirthDate >= '1930-01-01')
)

GO

CREATE TRIGGER [dbo].[OnDeleteStudent]
    ON [dbo].[Student]
    INSTEAD of DELETE
    AS
    BEGIN
        SET NoCount ON
        update Student set Active = 0 where Id in (select Id from deleted)
    END
GO
