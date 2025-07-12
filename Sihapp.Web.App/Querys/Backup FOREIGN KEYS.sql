ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD FOREIGN KEY([ClinicID])
REFERENCES [dbo].[Clinic] ([ID])
GO

ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD FOREIGN KEY([ClinicMenID])
REFERENCES [dbo].[ClinicMen] ([ID])
GO

ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD FOREIGN KEY([CreatedClinicMen])
REFERENCES [dbo].[ClinicMen] ([ID])
GO

ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD FOREIGN KEY([CreatedAuxiliar])
REFERENCES [dbo].[AuxiliarPersonal] ([Id])
GO

ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([Id])
GO

--========================================================================
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO

--=============================================================================

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO

--===============================================================================

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO

--====================================================================================
ALTER TABLE [dbo].[AuxiliarPersonal]  WITH CHECK ADD FOREIGN KEY([ClinicID])
REFERENCES [dbo].[Clinic] ([ID])
GO

ALTER TABLE [dbo].[AuxiliarPersonal]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

--======================================================================================
ALTER TABLE [dbo].[ClinicMen]  WITH CHECK ADD FOREIGN KEY([ClinicID])
REFERENCES [dbo].[Clinic] ([ID])
GO

ALTER TABLE [dbo].[ClinicMen]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO


--==========================================================================================

ALTER TABLE [dbo].[Consult]  WITH CHECK ADD FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointment] ([Id])
GO

--==========================================================================================

ALTER TABLE [dbo].[ConsultImages]  WITH CHECK ADD  CONSTRAINT [FK_ClinicMenID] FOREIGN KEY([ClinicMenID])
REFERENCES [dbo].[ClinicMen] ([ID])
GO

ALTER TABLE [dbo].[ConsultImages] CHECK CONSTRAINT [FK_ClinicMenID]
GO



ALTER TABLE [dbo].[ConsultImages]  WITH CHECK ADD  CONSTRAINT [FK_ConsultImgClinic] FOREIGN KEY([ClinicID])
REFERENCES [dbo].[Clinic] ([ID])
GO

ALTER TABLE [dbo].[ConsultImages] CHECK CONSTRAINT [FK_ConsultImgClinic]
GO

ALTER TABLE [dbo].[ConsultImages]  WITH CHECK ADD  CONSTRAINT [FK_PatientID] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([Id])
GO

ALTER TABLE [dbo].[ConsultImages] CHECK CONSTRAINT [FK_PatientID]
GO
--==============================================================================================
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD FOREIGN KEY([ClinicID])
REFERENCES [dbo].[Clinic] ([ID])
GO

ALTER TABLE [dbo].[Patients]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

--============================================================================================
ALTER TABLE [dbo].[Remission]  WITH CHECK ADD FOREIGN KEY([ConsultID])
REFERENCES [dbo].[Consult] ([Id])
GO

--============================================================================================
ALTER TABLE [dbo].[Supplies]  WITH CHECK ADD  CONSTRAINT [FK_SuppliesClinic] FOREIGN KEY([ClinicID])
REFERENCES [dbo].[Clinic] ([ID])
GO

ALTER TABLE [dbo].[Supplies] CHECK CONSTRAINT [FK_SuppliesClinic]
GO

ALTER TABLE [dbo].[Supplies]  WITH CHECK ADD  CONSTRAINT [FK_SuppliesClinicMen] FOREIGN KEY([ClinicMenID])
REFERENCES [dbo].[ClinicMen] ([ID])
GO

ALTER TABLE [dbo].[Supplies] CHECK CONSTRAINT [FK_SuppliesClinicMen]
GO

--==================================================================================================
SET ANSI_PADDING ON
GO

/****** Object:  Index [UK_principal_name]    Script Date: 29/04/2018 01:42:17 p. m. ******/
ALTER TABLE [dbo].[sysdiagrams] ADD  CONSTRAINT [UK_principal_name] UNIQUE NONCLUSTERED 
(
	[principal_id] ASC,
	[name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO



--===============================================================================================
















