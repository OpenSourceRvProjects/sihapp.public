Create table Clinic (

ID uniqueidentifier not null primary key, 
Name nvarchar(256) not null, 
Number int not null, 
Phone nvarchar(128) not null,
Address nvarchar(MAX) not null, 
Image varbinary null, 
RFC nvarchar(128)

)