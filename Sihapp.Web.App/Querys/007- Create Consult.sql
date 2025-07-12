Create table Consult (
Id uniqueidentifier not null primary key,
AppointmentID uniqueidentifier not null, 
ConsultStartTime datetime not null, 
ConsultEndTime datetime null, 
Notes nvarchar(MAX) null, 
Amount decimal not null, 

)

ALTER TABLE Consult ADD FOREIGN KEY (AppointmentID) REFERENCES Appointment(ID)
alter table consult add Number int not null