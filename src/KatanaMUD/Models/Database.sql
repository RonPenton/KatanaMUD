USE [KatanaMUD]
GO
/****** Object:  Table [dbo].[Actor]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actor](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NULL,
	[ActorType] [int] NOT NULL,
	[UserId] [nvarchar](50) NULL,
	[RoomId] [int] NOT NULL,
	[ClassTemplateId] [int] NULL,
	[RaceTemplateId] [int] NULL,
	[CharacterPoints] [int] NOT NULL,
	[JSONStats] [nvarchar](max) NULL,
 CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArmorType]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArmorType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ArmorType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClassTemplate]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassTemplate](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[JSONStats] [nvarchar](max) NULL,
 CONSTRAINT [PK_ClassTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClassTemplateArmorType]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassTemplateArmorType](
	[ClassTemplateId] [int] NOT NULL,
	[ArmorTypeId] [int] NOT NULL,
 CONSTRAINT [PK_ClassTemplateArmorType] PRIMARY KEY CLUSTERED 
(
	[ClassTemplateId] ASC,
	[ArmorTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClassTemplateWeaponType]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassTemplateWeaponType](
	[ClassTemplateId] [int] NOT NULL,
	[WeaponTypeId] [int] NOT NULL,
 CONSTRAINT [PK_ClassTemplateWeaponType] PRIMARY KEY CLUSTERED 
(
	[ClassTemplateId] ASC,
	[WeaponTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RaceClassRestriction]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RaceClassRestriction](
	[RaceTemplateId] [int] NOT NULL,
	[ClassTemplateId] [int] NOT NULL,
 CONSTRAINT [PK_RaceClassRestrictions] PRIMARY KEY CLUSTERED 
(
	[RaceTemplateId] ASC,
	[ClassTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RaceTemplate]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RaceTemplate](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[JSONStats] [nvarchar](max) NULL,
 CONSTRAINT [PK_RaceTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [nvarchar](50) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[LockoutEnd] [datetime] NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[IsConfirmed] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WeaponType]    Script Date: 4/10/2015 7:59:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeaponType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WeaponType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_ClassTemplate] FOREIGN KEY([ClassTemplateId])
REFERENCES [dbo].[ClassTemplate] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_ClassTemplate]
GO
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_RaceTemplate] FOREIGN KEY([RaceTemplateId])
REFERENCES [dbo].[RaceTemplate] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_RaceTemplate]
GO
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_User]
GO
ALTER TABLE [dbo].[ClassTemplateArmorType]  WITH CHECK ADD  CONSTRAINT [FK_ClassTemplateArmorType_ArmorType] FOREIGN KEY([ArmorTypeId])
REFERENCES [dbo].[ArmorType] ([Id])
GO
ALTER TABLE [dbo].[ClassTemplateArmorType] CHECK CONSTRAINT [FK_ClassTemplateArmorType_ArmorType]
GO
ALTER TABLE [dbo].[ClassTemplateArmorType]  WITH CHECK ADD  CONSTRAINT [FK_ClassTemplateArmorType_ClassTemplate] FOREIGN KEY([ClassTemplateId])
REFERENCES [dbo].[ClassTemplate] ([Id])
GO
ALTER TABLE [dbo].[ClassTemplateArmorType] CHECK CONSTRAINT [FK_ClassTemplateArmorType_ClassTemplate]
GO
ALTER TABLE [dbo].[ClassTemplateWeaponType]  WITH CHECK ADD  CONSTRAINT [FK_ClassTemplateWeaponType_ClassTemplate] FOREIGN KEY([ClassTemplateId])
REFERENCES [dbo].[ClassTemplate] ([Id])
GO
ALTER TABLE [dbo].[ClassTemplateWeaponType] CHECK CONSTRAINT [FK_ClassTemplateWeaponType_ClassTemplate]
GO
ALTER TABLE [dbo].[ClassTemplateWeaponType]  WITH CHECK ADD  CONSTRAINT [FK_ClassTemplateWeaponType_WeaponType] FOREIGN KEY([WeaponTypeId])
REFERENCES [dbo].[WeaponType] ([Id])
GO
ALTER TABLE [dbo].[ClassTemplateWeaponType] CHECK CONSTRAINT [FK_ClassTemplateWeaponType_WeaponType]
GO
ALTER TABLE [dbo].[RaceClassRestriction]  WITH CHECK ADD  CONSTRAINT [FK_RaceClassRestrictions_ClassTemplate] FOREIGN KEY([ClassTemplateId])
REFERENCES [dbo].[ClassTemplate] ([Id])
GO
ALTER TABLE [dbo].[RaceClassRestriction] CHECK CONSTRAINT [FK_RaceClassRestrictions_ClassTemplate]
GO
ALTER TABLE [dbo].[RaceClassRestriction]  WITH CHECK ADD  CONSTRAINT [FK_RaceClassRestrictions_RaceTemplate] FOREIGN KEY([RaceTemplateId])
REFERENCES [dbo].[RaceTemplate] ([Id])
GO
ALTER TABLE [dbo].[RaceClassRestriction] CHECK CONSTRAINT [FK_RaceClassRestrictions_RaceTemplate]
GO
