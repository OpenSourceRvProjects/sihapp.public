Create table RecordItems (
Id uniqueidentifier not null primary key, 
RecordID uniqueidentifier not null, 
ConsultID uniqueidentifier not null, 
Content nvarchar(MAX) not null, 
Image varbinary(MAX) null,
CreationDate datetime not null, 
Type int not null
)

ALTER TABLE RecordItems ADD FOREIGN KEY (RecordID) REFERENCES Records(ID)
ALTER TABLE RecordItems ADD FOREIGN KEY (ConsultID) REFERENCES Consult(ID)