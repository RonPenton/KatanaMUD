USE [KatanaMUD]
GO
/****** Object:  Table [dbo].[ItemTemplate]    Script Date: 04/23/2015 08:48:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemTemplate](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[Type] [int] NOT NULL,
	[EquipType] [int] NULL,
	[WeaponType] [int] NULL,
	[Limit] [int] NULL,
	[Fixed] [bit] NOT NULL,
	[NotDroppable] [bit] NOT NULL,
	[DestroyOnDeath] [bit] NOT NULL,
	[NotRobable] [bit] NOT NULL,
	[Cost] [bigint] NOT NULL,
	[Level] [int] NOT NULL,
	[JSONStats] [nvarchar](max) NULL,
	[JSONRequirements] [nvarchar](max) NULL,
 CONSTRAINT [PK_ItemTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassTemplate]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RaceTemplate]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TextBlock]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 04/23/2015 08:48:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[Id] [int] NOT NULL,
	[RegionId] [int] NULL,
	[Name] [nvarchar](60) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RaceClassRestriction]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Actor]    Script Date: 04/23/2015 08:48:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 04/23/2015 08:48:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemTemplateId] [int] NOT NULL,
	[CustomName] [nvarchar](50) NULL,
	[ActorId] [uniqueidentifier] NULL,
	[RoomId] [int] NULL,
	[Modified] [bit] NOT NULL,
	[JSONStats] [nvarchar](max) NULL,
	[EquippedSlot] [int] NULL,
	[HiddenTime] [datetime] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Actor_ClassTemplate]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_ClassTemplate] FOREIGN KEY([ClassTemplateId])
REFERENCES [dbo].[ClassTemplate] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_ClassTemplate]
GO
/****** Object:  ForeignKey [FK_Actor_RaceTemplate]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_RaceTemplate] FOREIGN KEY([RaceTemplateId])
REFERENCES [dbo].[RaceTemplate] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_RaceTemplate]
GO
/****** Object:  ForeignKey [FK_Actor_Room]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_Room]
GO
/****** Object:  ForeignKey [FK_Actor_User]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Actor]  WITH CHECK ADD  CONSTRAINT [FK_Actor_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Actor] CHECK CONSTRAINT [FK_Actor_User]
GO
/****** Object:  ForeignKey [FK_Item_Actor]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Actor] FOREIGN KEY([ActorId])
REFERENCES [dbo].[Actor] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Actor]
GO
/****** Object:  ForeignKey [FK_Item_ItemTemplate]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemTemplate] FOREIGN KEY([ItemTemplateId])
REFERENCES [dbo].[ItemTemplate] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ItemTemplate]
GO
/****** Object:  ForeignKey [FK_Item_Room]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Room]
GO
/****** Object:  ForeignKey [FK_RaceClassRestrictions_ClassTemplate]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[RaceClassRestriction]  WITH CHECK ADD  CONSTRAINT [FK_RaceClassRestrictions_ClassTemplate] FOREIGN KEY([ClassTemplateId])
REFERENCES [dbo].[ClassTemplate] ([Id])
GO
ALTER TABLE [dbo].[RaceClassRestriction] CHECK CONSTRAINT [FK_RaceClassRestrictions_ClassTemplate]
GO
/****** Object:  ForeignKey [FK_RaceClassRestrictions_RaceTemplate]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[RaceClassRestriction]  WITH CHECK ADD  CONSTRAINT [FK_RaceClassRestrictions_RaceTemplate] FOREIGN KEY([RaceTemplateId])
REFERENCES [dbo].[RaceTemplate] ([Id])
GO
ALTER TABLE [dbo].[RaceClassRestriction] CHECK CONSTRAINT [FK_RaceClassRestrictions_RaceTemplate]
GO
/****** Object:  ForeignKey [FK_Room_Region]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_Region]
GO
/****** Object:  ForeignKey [FK_Room_TextBlock]    Script Date: 04/23/2015 08:48:05 ******/
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_TextBlock] FOREIGN KEY([TextBlockId])
REFERENCES [dbo].[TextBlock] ([Id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_TextBlock]
GO
