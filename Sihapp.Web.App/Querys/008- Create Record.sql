Create table Records (
Id uniqueidentifier not null primary key, 
PatientID uniqueidentifier not null, 
CreationDate datetime not null, 
Status int not null, 
Notes nvarchar(MAX) not null, 

)

ALTER TABLE Records ADD FOREIGN KEY (PatientID) REFERENCES Patients(ID)
Alter table Records add Number int not null