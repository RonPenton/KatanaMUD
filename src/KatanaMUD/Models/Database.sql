USE [master]
GO
/****** Object:  Database [KatanaMUD]    Script Date: 5/4/2015 8:32:19 AM ******/
CREATE DATABASE [KatanaMUD]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KatanaMUD', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\KatanaMUD.mdf' , SIZE = 9216KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'KatanaMUD_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\KatanaMUD_log.ldf' , SIZE = 26816KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [KatanaMUD] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KatanaMUD].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KatanaMUD] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KatanaMUD] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KatanaMUD] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KatanaMUD] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KatanaMUD] SET ARITHABORT OFF 
GO
ALTER DATABASE [KatanaMUD] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [KatanaMUD] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KatanaMUD] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KatanaMUD] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KatanaMUD] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KatanaMUD] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KatanaMUD] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KatanaMUD] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KatanaMUD] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KatanaMUD] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KatanaMUD] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KatanaMUD] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KatanaMUD] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KatanaMUD] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KatanaMUD] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KatanaMUD] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KatanaMUD] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KatanaMUD] SET RECOVERY FULL 
GO
ALTER DATABASE [KatanaMUD] SET  MULTI_USER 
GO
ALTER DATABASE [KatanaMUD] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KatanaMUD] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KatanaMUD] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KatanaMUD] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [KatanaMUD] SET DELAYED_DURABILITY = DISABLED 
GO
USE [KatanaMUD]
GO
/****** Object:  Table [dbo].[Actor]    Script Date: 5/4/2015 8:32:20 AM ******/
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
	[JSONCash] [nvarchar](max) NULL,
 CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClassTemplate]    Script Date: 5/4/2015 8:32:20 AM ******/
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
/****** Object:  Table [dbo].[Currency]    Script Date: 5/4/2015 8:32:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ShortName] [nvarchar](50) NOT NULL,
	[Value] [bigint] NOT NULL,
	[Weight] [float] NOT NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Item]    Script Date: 5/4/2015 8:32:20 AM ******/
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
	[HiddenTime] [bigint] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemTemplate]    Script Date: 5/4/2015 8:32:20 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RaceClassRestriction]    Script Date: 5/4/2015 8:32:20 AM ******/
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
/****** Object:  Table [dbo].[RaceTemplate]    Script Date: 5/4/2015 8:32:20 AM ******/
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
/****** Object:  Table [dbo].[Region]    Script Date: 5/4/2015 8:32:20 AM ******/
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
/****** Object:  Table [dbo].[Room]    Script Date: 5/4/2015 8:32:20 AM ******/
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
	[JSONCash] [nvarchar](max) NULL,
	[JSONHiddenCash] [nvarchar](max) NULL,
	[JSONStats] [nvarchar](max) NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Setting]    Script Date: 5/4/2015 8:32:20 AM ******/
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
/****** Object:  Table [dbo].[TextBlock]    Script Date: 5/4/2015 8:32:20 AM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 5/4/2015 8:32:20 AM ******/
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
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Actor] FOREIGN KEY([ActorId])
REFERENCES [dbo].[Actor] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Actor]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemTemplate] FOREIGN KEY([ItemTemplateId])
REFERENCES [dbo].[ItemTemplate] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ItemTemplate]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Room]
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
USE [master]
GO
ALTER DATABASE [KatanaMUD] SET  READ_WRITE 
GO
