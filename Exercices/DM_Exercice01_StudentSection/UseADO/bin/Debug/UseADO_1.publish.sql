/*
Script de déploiement pour ADO_choupette

Ce code a été généré par un outil.
La modification de ce fichier peut provoquer un comportement incorrect et sera perdue si
le code est régénéré.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "ADO_choupette"
:setvar DefaultFilePrefix "ADO_choupette"
:setvar DefaultDataPath "C:\Program Files\Micosoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Micosoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Détectez le mode SQLCMD et désactivez l'exécution du script si le mode SQLCMD n'est pas pris en charge.
Pour réactiver le script une fois le mode SQLCMD activé, exécutez ce qui suit :
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Le mode SQLCMD doit être activé de manière à pouvoir exécuter ce script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Suppression de [dbo].[CK_Student_YearResult_0_20]...';


GO
ALTER TABLE [dbo].[Student] DROP CONSTRAINT [CK_Student_YearResult_0_20];


GO
PRINT N'Création de [dbo].[CK_Student_YearResult_0_20]...';


GO
ALTER TABLE [dbo].[Student] WITH NOCHECK
    ADD CONSTRAINT [CK_Student_YearResult_0_20] CHECK (YearResult between 0 and 20);


GO
PRINT N'Modification de [dbo].[Trigger_Student]...';


GO

ALTER TRIGGER [dbo].[Trigger_Student]
    ON [dbo].[Student]
    INSTEAD of DELETE
    AS
    BEGIN
        SET NoCount ON
        update Student set Active = 0 where Id in (select * from deleted)
    END
GO
PRINT N'Création de [dbo].[AddStudent]...';


GO
CREATE PROCEDURE [dbo].[AddStudent]

	@FirstName varchar(50),
	@LastName varchar(50),
	@BirthDate DateTime2(7),
	@YearResult int,
	@SectionId int

AS
BEGIN

	insert into Student 
		(FirstName,LastName, BirthDate, YearResult, SectionID)
	output inserted.Id
	values 
		(@FirstName,@LastName, @BirthDate,@YearResult, @SectionId)

END
GO
PRINT N'Création de [dbo].[DeleteStudent]...';


GO
CREATE PROCEDURE [dbo].[DeleteStudent]
	
	@Id int

AS
BEGIN

	delete from Student 
	where Id = @Id

END
GO
PRINT N'Création de [dbo].[UpdateStudent]...';


GO
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
GO
/*
Modèle de script de post-déploiement							
--------------------------------------------------------------------------------------
 Ce fichier contient des instructions SQL qui seront ajoutées au script de compilation.		
 Utilisez la syntaxe SQLCMD pour inclure un fichier dans le script de post-déploiement.			
 Exemple :      :r .\monfichier.sql								
 Utilisez la syntaxe SQLCMD pour référencer une variable dans le script de post-déploiement.		
 Exemple :      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
GO

GO
PRINT N'Vérification de données existantes par rapport aux nouvelles contraintes';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Student] WITH CHECK CHECK CONSTRAINT [CK_Student_YearResult_0_20];


GO
PRINT N'Mise à jour terminée.';


GO
