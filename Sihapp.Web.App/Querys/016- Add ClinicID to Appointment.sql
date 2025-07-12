alter table Appointment add ClinicID uniqueidentifier not null;

ALTER TABLE Appointment
ADD FOREIGN KEY (ClinicID) REFERENCES Clinic(ID);