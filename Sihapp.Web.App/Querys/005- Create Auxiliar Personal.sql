Create table AuxiliarPersonal (
Id uniqueidentifier not null primary key,
ClinicID uniqueidentifier not null, 
UserID nvarchar(128) not null, 
Name nvarchar(MAX) not null,
LastName1 nvarchar(MAX) not null,
LastName2 nvarchar(MAX) not null, 
BirthDate datetime not null, 
CreationDate datetime not null, 
HireDate datetime not null, 
Photo varbinary (MAX) null, 
Address nvarchar(MAX) not null, 

)

ALTER TABLE AuxiliarPersonal ADD FOREIGN KEY (UserID) REFERENCES AspNetUsers(ID)
ALTER TABLE AuxiliarPersonal ADD FOREIGN KEY (ClinicID) REFERENCES Clinic(ID)