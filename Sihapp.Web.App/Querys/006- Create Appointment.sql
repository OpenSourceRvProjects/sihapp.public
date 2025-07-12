Create table Appointment (
Id uniqueidentifier not null, 
CreatedClinicMen uniqueidentifier null, 
CreatedAuxiliar uniqueidentifier null,
ClinicMenID uniqueidentifier not null, 
PatientID uniqueIdentifier not null, 
AppointmentDate datetime not null, 
Address nvarchar(MAX) not null,
Comments nvarchar(MAX) not null, 

)
ALTER TABLE Appointment ADD PRIMARY KEY (ID)

--relacion paciente- doctor
ALTER TABLE Appointment ADD FOREIGN KEY (PatientID) REFERENCES Patients(ID)
ALTER TABLE Appointment ADD FOREIGN KEY (ClinicMenID) REFERENCES ClinicMen(ID)

--relacion quien hizo la cita
ALTER TABLE Appointment ADD FOREIGN KEY (CreatedClinicMen) REFERENCES ClinicMen(ID)
ALTER TABLE Appointment ADD FOREIGN KEY (CreatedAuxiliar) REFERENCES AuxiliarPersonal(ID)

alter table Appointment Add Type int not null
alter table Appointment Add CreationDate datetime not null
alter table Appointment Add Status int not null
alter table Appointment Add Number int not null