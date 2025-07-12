SET ANSI_PADDING ON
GO

/****** Object:  Index [PK_dbo.__MigrationHistory]    Script Date: 29/04/2018 01:20:19 p. m. ******/
ALTER TABLE [dbo].[__MigrationHistory] ADD  CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO


/****** Object:  Index [PK__Appointm__3214EC077807340B]    Script Date: 29/04/2018 01:20:57 p. m. ******/
ALTER TABLE [dbo].[Appointment] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO


SET ANSI_PADDING ON
GO

/****** Object:  Index [PK_dbo.AspNetRoles]    Script Date: 29/04/2018 01:21:48 p. m. ******/
ALTER TABLE [dbo].[AspNetRoles] ADD  CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK_dbo.AspNetUserClaims]    Script Date: 29/04/2018 01:22:29 p. m. ******/
ALTER TABLE [dbo].[AspNetUserClaims] ADD  CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [PK_dbo.AspNetUserLogins]    Script Date: 29/04/2018 01:22:51 p. m. ******/
ALTER TABLE [dbo].[AspNetUserLogins] ADD  CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [PK_dbo.AspNetUserRoles]    Script Date: 29/04/2018 01:23:12 p. m. ******/
ALTER TABLE [dbo].[AspNetUserRoles] ADD  CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [PK_dbo.AspNetUsers]    Script Date: 29/04/2018 01:23:34 p. m. ******/
ALTER TABLE [dbo].[AspNetUsers] ADD  CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__Auxiliar__3214EC07D4ED52CB]    Script Date: 29/04/2018 01:24:01 p. m. ******/
ALTER TABLE [dbo].[AuxiliarPersonal] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__Clinic__3214EC276C98F51D]    Script Date: 29/04/2018 01:24:22 p. m. ******/
ALTER TABLE [dbo].[Clinic] ADD PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__ClinicMe__3214EC2741C9B994]    Script Date: 29/04/2018 01:24:49 p. m. ******/
ALTER TABLE [dbo].[ClinicMen] ADD PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__Consult__3214EC07F8C2EFFE]    Script Date: 29/04/2018 01:25:13 p. m. ******/
ALTER TABLE [dbo].[Consult] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__ConsultI__3214EC071B881DC8]    Script Date: 29/04/2018 01:25:35 p. m. ******/
ALTER TABLE [dbo].[ConsultImages] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__Patients__3214EC075560F691]    Script Date: 29/04/2018 01:25:58 p. m. ******/
ALTER TABLE [dbo].[Patients] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Remission]  WITH CHECK ADD FOREIGN KEY([ConsultID])
REFERENCES [dbo].[Consult] ([Id])
GO

/****** Object:  Index [PK__Sucessfu__3214EC2795CFF5F8]    Script Date: 29/04/2018 01:27:13 p. m. ******/
ALTER TABLE [dbo].[SucessfullLogins] ADD PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__Supplies__3214EC07FEF5F4E7]    Script Date: 29/04/2018 01:27:30 p. m. ******/
ALTER TABLE [dbo].[Supplies] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [PK__sysdiagr__C2B05B61EE639044]    Script Date: 29/04/2018 01:27:56 p. m. ******/
ALTER TABLE [dbo].[sysdiagrams] ADD PRIMARY KEY CLUSTERED 
(
	[diagram_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO


















