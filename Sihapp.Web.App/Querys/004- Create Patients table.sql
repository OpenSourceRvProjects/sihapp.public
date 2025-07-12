USE [SihappDB]
GO


CREATE TABLE [dbo].[Patients](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[LastName1] [nvarchar](128) NOT NULL,
	[LastName2] [nvarchar](128) NOT NULL,
	[ClinicID] [uniqueidentifier] NOT NULL,
	[UserID] [nvarchar](128) NULL,
	[Birthdate] [datetime] NOT NULL,
	[Deathdate] [datetime] NULL,
	[Weight] [nvarchar](128) NULL,
	[Height] [nvarchar](128) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Photo] [varbinary](1) NULL,
	[PatientNumber] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

ALTER TABLE Patients ADD FOREIGN KEY (UserID) REFERENCES AspNetUsers(ID)

ALTER TABLE Patients ADD FOREIGN KEY (ClinicID) REFERENCES Clinic(ID)
