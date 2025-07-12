Create table Remission (
Id uniqueidentifier not null primary key,
ConsultID uniqueidentifier not null, 
GrandTotal decimal not null,
VAT decimal not null, 
PatientID uniqueidentifier not null, 
Notes nvarchar(MAX) not null, 
Number int not null, 

)

alter table Remission add Foreign key (ConsultID) references Consult(ID)
--alter table Remission add foreign key (PatientID) references Patients(ID)

--alter table Remission add ClinicMenID uniqueidentifier not null
--alter table Remission add foreign key (ClinicMenID) references ClinicMen(ID)