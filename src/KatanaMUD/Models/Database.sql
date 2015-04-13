USE [KatanaMUD]
GO
/****** Object:  Table [dbo].[Actor]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[ArmorType]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[ClassTemplate]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[ClassTemplateArmorType]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[ClassTemplateWeaponType]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[RaceClassRestriction]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[RaceTemplate]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[Region]    Script Date: 4/13/2015 8:45:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Room]    Script Date: 4/13/2015 8:45:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[Id] [int] NOT NULL,
	[RegionId] [int] NULL,
	[Name] [nvarchar](60) NOT NULL,
	[TextBlockId] [int] NOT NULL,
	[NorthExit] [int] NULL,
	[SouthExit] [int] NULL,
	[EastExit] [int] NULL,
	[WestExit] [int] NULL,
	[NorthEastExit] [int] NULL,
	[NorthWestExit] [int] NULL,
	[SouthEastExit] [int] NULL,
	[SouthWestExit] [int] NULL,
	[UpExit] [int] NULL,
	[DownExit] [int] NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Setting]    Script Date: 4/13/2015 8:45:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[Id] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TextBlock]    Script Date: 4/13/2015 8:45:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TextBlock](
	[Id] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TextBlock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 4/13/2015 8:45:56 AM ******/
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
/****** Object:  Table [dbo].[WeaponType]    Script Date: 4/13/2015 8:45:56 AM ******/
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
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_Room]
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
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_Region]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_TextBlock] FOREIGN KEY([TextBlockId])
REFERENCES [dbo].[TextBlock] ([Id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_TextBlock]
GO
