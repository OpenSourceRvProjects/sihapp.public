
ALTER TABLE [dbo].[Appointment] DROP CONSTRAINT [FK__Appointme__Consu__7E37BEF6]
GO


ALTER TABLE APPOINTMENT drop column ConsultID

CREATE UNIQUE INDEX CONSULT_UNIQUE_INDEX   
   ON Consult (AppointmentID);  

 drop table RecordItems

 drop table Records
