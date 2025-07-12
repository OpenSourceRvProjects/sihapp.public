alter table Appointment add ConsultID uniqueidentifier null

ALTER TABLE Appointment
ADD FOREIGN KEY (ConsultID) REFERENCES Consult(ID);