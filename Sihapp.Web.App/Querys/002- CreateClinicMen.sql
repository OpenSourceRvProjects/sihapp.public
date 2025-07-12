Create table ClinicMen (
ID uniqueidentifier not null primary key,
UserID nvarchar(128) not null, 
ClinicID uniqueidentifier not null,
Type int not null, 
Name nvarchar(128) not null, 
LastName1 nvarchar(128) not null,
LastName2 nvarchar(128) not null, 
Phone nvarchar(128) null, 
Address nvarchar(MAX) not null,
CellPhone nvarchar(128) null, 
Photo Varbinary null,
RFC nvarchar(128) null,
Cedula nvarchar(128) null,
Specialty nvarchar(128) null,
CreationDate datetime not null, 
HireDate datetime not null,
BirthDate datetime not null, 

)

ALTER TABLE ClinicMen
ADD FOREIGN KEY (UserID) REFERENCES AspNetUsers(ID);

ALTER TABLE ClinicMen
ADD FOREIGN KEY (ClinicID) REFERENCES Clinic(ID);

