USE [master]
GO
/****** Object:  Database [apartment]    Script Date: 7/2/2020 8:46:33 PM ******/
CREATE DATABASE [apartment]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'apartment', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\apartment.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'apartment_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\apartment_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [apartment] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [apartment].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [apartment] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [apartment] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [apartment] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [apartment] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [apartment] SET ARITHABORT OFF 
GO
ALTER DATABASE [apartment] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [apartment] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [apartment] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [apartment] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [apartment] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [apartment] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [apartment] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [apartment] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [apartment] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [apartment] SET  DISABLE_BROKER 
GO
ALTER DATABASE [apartment] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [apartment] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [apartment] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [apartment] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [apartment] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [apartment] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [apartment] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [apartment] SET RECOVERY FULL 
GO
ALTER DATABASE [apartment] SET  MULTI_USER 
GO
ALTER DATABASE [apartment] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [apartment] SET DB_CHAINING OFF 
GO
ALTER DATABASE [apartment] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [apartment] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [apartment] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'apartment', N'ON'
GO
ALTER DATABASE [apartment] SET QUERY_STORE = OFF
GO
USE [apartment]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [apartment]
GO
/****** Object:  Table [dbo].[accounts]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[accounts](
	[idUser] [int] IDENTITY(1000000,1) NOT NULL,
	[Username] [nvarchar](25) NOT NULL,
	[Sex] [nvarchar](3) NOT NULL,
	[Birthday] [smalldatetime] NOT NULL,
	[IDCard] [varchar](12) NULL,
	[idStaff] [int] NOT NULL,
	[lastLogin] [datetime] NOT NULL,
	[RegDate] [datetime] NOT NULL,
	[Image_Avatar] [nvarchar](max) NULL,
	[Image_IDCard1] [nvarchar](max) NULL,
	[Image_IDCard2] [nvarchar](max) NULL,
 CONSTRAINT [PK_accounts] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[apartments]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[apartments](
	[idMain] [int] IDENTITY(1000000,1) NOT NULL,
	[name] [nvarchar](30) NOT NULL,
	[statusGeneral] [int] NOT NULL,
	[idSub] [int] NOT NULL,
	[description] [nvarchar](50) NULL,
	[typeApartment] [int] NOT NULL,
	[Rent] [decimal](19, 4) NULL,
	[DateRent] [datetime] NULL,
	[ExpRent] [datetime] NULL,
	[DienTich] [int] NOT NULL,
 CONSTRAINT [PK_apartment] PRIMARY KEY CLUSTERED 
(
	[idMain] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[apartments_type]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[apartments_type](
	[idType] [int] IDENTITY(1,1) NOT NULL,
	[name_type] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_apartments_type] PRIMARY KEY CLUSTERED 
(
	[idType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InfoApartment]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InfoApartment](
	[id] [int] NOT NULL,
	[nameApartment] [nvarchar](30) NOT NULL,
	[addressApartment] [nvarchar](50) NOT NULL,
	[telApartment] [nvarchar](15) NOT NULL,
	[emailApartment] [varchar](40) NOT NULL,
	[tittleNavbar] [nvarchar](15) NOT NULL,
	[imageNavbar] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_InfoApartment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InfoBank]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InfoBank](
	[idInfoBank] [int] IDENTITY(1,1) NOT NULL,
	[nameBank] [nvarchar](60) NOT NULL,
	[numberBank] [nvarchar](20) NOT NULL,
	[ChiNhanhBank] [nvarchar](30) NOT NULL,
	[ownerBank] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_InfoBank] PRIMARY KEY CLUSTERED 
(
	[idInfoBank] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[log_reasonCancel]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[log_reasonCancel](
	[idLog_Service] [int] NOT NULL,
	[Reason] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_log_reasonCancel_1] PRIMARY KEY CLUSTERED 
(
	[idLog_Service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[log_service]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[log_service](
	[idLogService] [int] IDENTITY(1,1) NOT NULL,
	[nameService] [nvarchar](25) NOT NULL,
	[RegDate] [datetime] NOT NULL,
	[ExpDate] [datetime] NOT NULL,
	[HeSo] [float] NOT NULL,
	[Price] [decimal](19, 4) NOT NULL,
	[DonVi] [varchar](10) NULL,
	[type] [bit] NOT NULL,
	[idHome] [int] NOT NULL,
	[CancelOrSave] [bit] NOT NULL,
	[idUserCancel] [int] NOT NULL,
 CONSTRAINT [PK_log_service] PRIMARY KEY CLUSTERED 
(
	[idLogService] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[login]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[login](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[Email] [varchar](50) NULL,
	[Password] [varchar](32) NULL,
	[PhoneNumber] [varchar](15) NULL,
 CONSTRAINT [PK_login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[notifications_general]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notifications_general](
	[idNotify] [int] IDENTITY(100000,1) NOT NULL,
	[notify] [nvarchar](max) NOT NULL,
	[date_submited] [datetime] NOT NULL,
	[idUser] [int] NOT NULL,
	[typeUser] [int] NOT NULL,
 CONSTRAINT [PK_notifications_general] PRIMARY KEY CLUSTERED 
(
	[idNotify] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[owner_home]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[owner_home](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[idHome] [int] NOT NULL,
 CONSTRAINT [PK_owner_home] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[position]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[position](
	[idStaff] [int] NOT NULL,
	[name_position] [nvarchar](25) NOT NULL,
	[idLevel] [int] NOT NULL,
 CONSTRAINT [PK_position] PRIMARY KEY CLUSTERED 
(
	[idStaff] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[services]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[services](
	[idService] [int] IDENTITY(1000000,1) NOT NULL,
	[RegDate] [datetime] NOT NULL,
	[ExpDate] [datetime] NOT NULL,
	[serviceType] [int] NOT NULL,
	[idHome] [int] NOT NULL,
	[value] [float] NOT NULL,
 CONSTRAINT [PK_services] PRIMARY KEY CLUSTERED 
(
	[idService] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[services_type]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[services_type](
	[serviceType] [int] IDENTITY(1,1) NOT NULL,
	[name_service] [nvarchar](25) NOT NULL,
	[description] [nvarchar](200) NULL,
	[Price] [decimal](19, 4) NOT NULL,
	[type] [bit] NOT NULL,
	[DonVi] [varchar](10) NULL,
 CONSTRAINT [PK_service_type] PRIMARY KEY CLUSTERED 
(
	[serviceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[statusGeneral]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[statusGeneral](
	[idStatus] [int] IDENTITY(1,1) NOT NULL,
	[nameStatus] [nvarchar](25) NOT NULL,
	[idGroup] [int] NOT NULL,
 CONSTRAINT [PK__statusGe__01936F741996F88F] PRIMARY KEY CLUSTERED 
(
	[idStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tickets]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tickets](
	[idTicket] [int] IDENTITY(1000000,1) NOT NULL,
	[typeTicket] [int] NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[idtStatus] [int] NOT NULL,
	[TittleTicket] [nvarchar](50) NOT NULL,
	[idUserCreate] [int] NOT NULL,
	[idUsetLastPost] [int] NOT NULL,
	[Closed] [bit] NOT NULL,
 CONSTRAINT [PK_tickets] PRIMARY KEY CLUSTERED 
(
	[idTicket] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tickets_text]    Script Date: 7/2/2020 8:46:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tickets_text](
	[idtText] [int] IDENTITY(1,1) NOT NULL,
	[idUser_post] [int] NOT NULL,
	[DateSent] [datetime] NOT NULL,
	[tText] [nvarchar](max) NOT NULL,
	[idTicket] [int] NOT NULL,
 CONSTRAINT [PK_tickets_text] PRIMARY KEY CLUSTERED 
(
	[idtText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[accounts] ON 

INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (100001, N'Đỗ Ngọc Sỹ', N'Nam', CAST(N'1999-05-05T00:00:00' AS SmallDateTime), N'079033554638', 3, CAST(N'2020-06-25T07:44:16.000' AS DateTime), CAST(N'2020-04-11T00:01:48.000' AS DateTime), N'24062020225012b.jpg', N'04062020010258backgroundDefault.jpg', N'04062020010258christmas_toys_christmas_new_year_131953_1920x1080.jpg')
INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (1000004, N'Kinh Doanh', N'Nam', CAST(N'1999-01-01T00:00:00' AS SmallDateTime), N'098900335898', 1, CAST(N'2020-06-08T21:20:07.000' AS DateTime), CAST(N'2020-06-04T01:03:35.000' AS DateTime), NULL, N'04062020010335branches_garland_spruce_118627_1920x1080.jpg', N'04062020010335fence_dark_night_154238_1920x1080.jpg')
INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (1000005, N'Quản Lý', N'Nam', CAST(N'1999-01-01T00:00:00' AS SmallDateTime), N'096988778987', 2, CAST(N'2020-06-08T21:22:18.000' AS DateTime), CAST(N'2020-06-04T01:04:06.000' AS DateTime), NULL, N'04062020010406christmas_toys_christmas_new_year_131953_1920x1080.jpg', N'04062020010406fence_dark_night_154238_1920x1080.jpg')
INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (1000006, N'Khách hàng Một', N'Nam', CAST(N'1999-01-01T00:00:00' AS SmallDateTime), N'897800668756', 0, CAST(N'2020-06-25T07:43:11.000' AS DateTime), CAST(N'2020-06-04T01:07:55.000' AS DateTime), NULL, N'04062020010755branches_garland_spruce_118627_1920x1080.jpg', N'04062020010755fence_dark_night_154238_1920x1080.jpg')
INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (1000007, N'Khách hàng Hai', N'Nam', CAST(N'1999-01-01T00:00:00' AS SmallDateTime), NULL, 0, CAST(N'2020-06-04T01:09:32.000' AS DateTime), CAST(N'2020-06-04T01:09:32.000' AS DateTime), NULL, N'04062020010932christmas_toys_christmas_new_year_131953_1920x1080.jpg', N'04062020010932fence_dark_night_154238_1920x1080.jpg')
INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (1000008, N'Khách hàng Ba', N'Nam', CAST(N'1999-01-01T00:00:00' AS SmallDateTime), NULL, 0, CAST(N'2020-06-04T01:10:07.000' AS DateTime), CAST(N'2020-06-04T01:10:07.000' AS DateTime), NULL, N'04062020011007christmas_toys_christmas_new_year_131953_1920x1080.jpg', N'04062020011007fence_dark_night_154238_1920x1080.jpg')
INSERT [dbo].[accounts] ([idUser], [Username], [Sex], [Birthday], [IDCard], [idStaff], [lastLogin], [RegDate], [Image_Avatar], [Image_IDCard1], [Image_IDCard2]) VALUES (1000009, N'Khách hàng nè', N'Nam', CAST(N'1999-01-01T00:00:00' AS SmallDateTime), N'097999887085', 0, CAST(N'2020-06-24T23:29:13.000' AS DateTime), CAST(N'2020-06-24T23:29:13.000' AS DateTime), NULL, N'24062020232913branches_garland_spruce_118627_1920x1080.jpg', N'24062020232913fence_dark_night_154238_1920x1080.jpg')
SET IDENTITY_INSERT [dbo].[accounts] OFF
SET IDENTITY_INSERT [dbo].[apartments] ON 

INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10001, N'NSY Valley 1', 1, 0, N'32 Nguyễn xí, Phường 3, Quận Bình Thạnh, TP.HCM', 1, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10002, N'Block A', 1, 10001, NULL, 2, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10003, N'Tầng 1', 1, 10002, NULL, 3, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10006, N'NSY Valley 2', 2, 0, N'354 Dương quảng hàm, Phường 5, Quận Gò vấp, TP.HCM', 1, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10007, N'Block A', 2, 10006, NULL, 2, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10009, N'A.01-01', 1, 10008, NULL, 4, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (10010, N'A.01-02', 2, 10008, NULL, 4, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (11007, N'A.01-03', 3, 10003, NULL, 4, CAST(6500000.0000 AS Decimal(19, 4)), CAST(N'2020-06-24T00:00:00.000' AS DateTime), CAST(N'2020-06-27T00:00:00.000' AS DateTime), 50)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (11008, N'A.01-04', 4, 10003, NULL, 4, NULL, NULL, NULL, 50)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (11009, N'A.01-02', 1, 10003, NULL, 4, NULL, NULL, NULL, 50)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (11010, N'A.01-01', 3, 10003, NULL, 4, CAST(10000000.0000 AS Decimal(19, 4)), CAST(N'2020-05-07T00:00:00.000' AS DateTime), CAST(N'2020-06-07T00:00:00.000' AS DateTime), 50)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000000, N'Tầng 1', 2, 10007, NULL, 3, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000001, N'A.01-01', 2, 1000000, NULL, 4, NULL, NULL, NULL, 40)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000002, N'A.01-02', 1, 1000000, NULL, 4, NULL, NULL, NULL, 40)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000003, N'NSY Valley 3', 5, 0, NULL, 1, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000004, N'Block B', 2, 10001, NULL, 2, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000005, N'Tầng 1', 2, 1000004, NULL, 3, NULL, NULL, NULL, 0)
INSERT [dbo].[apartments] ([idMain], [name], [statusGeneral], [idSub], [description], [typeApartment], [Rent], [DateRent], [ExpRent], [DienTich]) VALUES (1000006, N'B.01-01', 2, 1000005, NULL, 4, NULL, NULL, NULL, 40)
SET IDENTITY_INSERT [dbo].[apartments] OFF
SET IDENTITY_INSERT [dbo].[apartments_type] ON 

INSERT [dbo].[apartments_type] ([idType], [name_type]) VALUES (1, N'Chưng cư')
INSERT [dbo].[apartments_type] ([idType], [name_type]) VALUES (2, N'Block')
INSERT [dbo].[apartments_type] ([idType], [name_type]) VALUES (3, N'Tầng')
INSERT [dbo].[apartments_type] ([idType], [name_type]) VALUES (4, N'Home')
SET IDENTITY_INSERT [dbo].[apartments_type] OFF
INSERT [dbo].[InfoApartment] ([id], [nameApartment], [addressApartment], [telApartment], [emailApartment], [tittleNavbar], [imageNavbar]) VALUES (0, N'NSY Apartment', N'Vũ trụ ngoài trái đất', N'0586209147', N'nsy.apartment@gmail.com', N'NSY Apartment', N'29052020221826Logo.png')
SET IDENTITY_INSERT [dbo].[InfoBank] ON 

INSERT [dbo].[InfoBank] ([idInfoBank], [nameBank], [numberBank], [ChiNhanhBank], [ownerBank]) VALUES (2, N'Ngân Hàng TMCP Ngoại Thương Việt Nam (Vietcombank)', N'053 100 248 2157', N'Đông Sài Gòn (PGD: Thảo Điền)', N'Tòa nhà NSY Apartment')
INSERT [dbo].[InfoBank] ([idInfoBank], [nameBank], [numberBank], [ChiNhanhBank], [ownerBank]) VALUES (3, N'Ngân Hàng TMCP Kỹ Thương Việt Nam (Techcombank)', N'191 341 052 670 18', N'Gia Định', N'Tòa nhà NSY Apartment')
SET IDENTITY_INSERT [dbo].[InfoBank] OFF
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (12, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (14, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (15, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (16, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (17, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (18, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (19, N'Không đóng tiền')
INSERT [dbo].[log_reasonCancel] ([idLog_Service], [Reason]) VALUES (20, N'Không đóng tiền')
SET IDENTITY_INSERT [dbo].[log_service] ON 

INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (12, N'Dịch vụ Internet', CAST(N'2020-05-14T22:31:27.000' AS DateTime), CAST(N'2020-06-13T22:31:27.000' AS DateTime), 0, CAST(200000.0000 AS Decimal(19, 4)), NULL, 0, 11010, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (14, N'Dịch vụ Internet', CAST(N'2020-05-14T22:31:27.000' AS DateTime), CAST(N'2020-06-13T22:31:27.000' AS DateTime), 0, CAST(200000.0000 AS Decimal(19, 4)), NULL, 0, 11010, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (15, N'Dịch vụ Nước', CAST(N'2020-05-18T20:38:40.000' AS DateTime), CAST(N'2020-06-17T20:38:40.000' AS DateTime), 0, CAST(4000.0000 AS Decimal(19, 4)), N'm3', 1, 11010, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (16, N'Dịch vụ Điện', CAST(N'2020-05-18T20:38:40.000' AS DateTime), CAST(N'2020-06-17T20:38:40.000' AS DateTime), 30, CAST(3000.0000 AS Decimal(19, 4)), N'kWh', 1, 11010, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (17, N'Dịch vụ Vệ sinh', CAST(N'2020-05-19T23:45:08.000' AS DateTime), CAST(N'2020-06-18T23:45:08.000' AS DateTime), 0, CAST(35000.0000 AS Decimal(19, 4)), NULL, 0, 11009, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (18, N'Dịch vụ Internet', CAST(N'2020-05-19T23:51:24.000' AS DateTime), CAST(N'2020-06-18T23:51:24.000' AS DateTime), 0, CAST(200000.0000 AS Decimal(19, 4)), NULL, 0, 11010, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (19, N'Dịch vụ Điện', CAST(N'2020-05-30T01:53:34.000' AS DateTime), CAST(N'2020-06-29T01:53:34.000' AS DateTime), 0, CAST(3000.0000 AS Decimal(19, 4)), N'kWh', 1, 11009, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (20, N'Dịch vụ Nước', CAST(N'2020-05-30T01:53:34.000' AS DateTime), CAST(N'2020-06-29T01:53:34.000' AS DateTime), 0, CAST(4000.0000 AS Decimal(19, 4)), N'm3', 1, 11009, 0, 100001)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (21, N'Dịch vụ Vệ sinh', CAST(N'2020-05-19T23:32:32.000' AS DateTime), CAST(N'2020-06-18T23:32:32.000' AS DateTime), 0, CAST(35000.0000 AS Decimal(19, 4)), NULL, 0, 11010, 1, 0)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (22, N'Dịch vụ Vệ sinh', CAST(N'2020-06-18T23:32:32.000' AS DateTime), CAST(N'2020-07-18T23:32:32.000' AS DateTime), 0, CAST(35000.0000 AS Decimal(19, 4)), NULL, 0, 11010, 1, 0)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (23, N'Dịch vụ Điện', CAST(N'2020-05-19T22:15:19.000' AS DateTime), CAST(N'2020-06-06T22:15:19.000' AS DateTime), 50, CAST(3000.0000 AS Decimal(19, 4)), N'kWh', 1, 11010, 1, 0)
INSERT [dbo].[log_service] ([idLogService], [nameService], [RegDate], [ExpDate], [HeSo], [Price], [DonVi], [type], [idHome], [CancelOrSave], [idUserCancel]) VALUES (24, N'Dịch vụ Điện', CAST(N'2020-06-06T22:15:19.000' AS DateTime), CAST(N'2020-06-26T22:15:19.000' AS DateTime), 150, CAST(3000.0000 AS Decimal(19, 4)), N'kWh', 1, 11010, 1, 0)
SET IDENTITY_INSERT [dbo].[log_service] OFF
SET IDENTITY_INSERT [dbo].[login] ON 

INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (1, 100001, N'bearof1999@gmail.com', N'202CB962AC59075B964B07152D234B70', N'0586209147')
INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (7, 1000004, N'kinhdoanh@gmail.com', N'202CB962AC59075B964B07152D234B70', N'0536879879')
INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (8, 1000005, N'quanly@gmail.com', N'202CB962AC59075B964B07152D234B70', N'0658978989')
INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (9, 1000006, N'khachhang1@gmail.com', N'202CB962AC59075B964B07152D234B70', N'0568789878')
INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (10, 1000007, NULL, NULL, NULL)
INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (11, 1000008, NULL, N'202CB962AC59075B964B07152D234B70', NULL)
INSERT [dbo].[login] ([id], [idUser], [Email], [Password], [PhoneNumber]) VALUES (12, 1000009, N'khachhangne2@gmail.com', N'202CB962AC59075B964B07152D234B70', N'0586206978')
SET IDENTITY_INSERT [dbo].[login] OFF
SET IDENTITY_INSERT [dbo].[notifications_general] ON 

INSERT [dbo].[notifications_general] ([idNotify], [notify], [date_submited], [idUser], [typeUser]) VALUES (100014, N'<h3 style="text-align: center; "><span style="font-family: &quot;Comic Sans MS&quot;; color: rgb(99, 74, 165);"><b>NSY Apartment</b></span></h3><h6 style="text-align: left;"><span style="font-family: &quot;Comic Sans MS&quot;;">1. Test</span></h6><h6 style="text-align: left;"><span style="font-family: &quot;Comic Sans MS&quot;;">2. Test 2</span></h6><h6 style="text-align: left;"><span style="font-family: &quot;Comic Sans MS&quot;;"><br></span></h6>', CAST(N'2020-06-24T22:44:54.000' AS DateTime), 100001, 0)
INSERT [dbo].[notifications_general] ([idNotify], [notify], [date_submited], [idUser], [typeUser]) VALUES (100015, N'<h3 style="font-family: &quot;Source Sans Pro&quot;, -apple-system, BlinkMacSystemFont, &quot;Segoe UI&quot;, Roboto, &quot;Helvetica Neue&quot;, Arial, sans-serif, &quot;Apple Color Emoji&quot;, &quot;Segoe UI Emoji&quot;, &quot;Segoe UI Symbol&quot;; color: rgb(0, 0, 0); text-align: center;"><span style="font-family: &quot;Comic Sans MS&quot;; color: rgb(99, 74, 165);"><span style="font-weight: bolder;">NSY Apartment</span></span></h3><h6 style="font-family: &quot;Source Sans Pro&quot;, -apple-system, BlinkMacSystemFont, &quot;Segoe UI&quot;, Roboto, &quot;Helvetica Neue&quot;, Arial, sans-serif, &quot;Apple Color Emoji&quot;, &quot;Segoe UI Emoji&quot;, &quot;Segoe UI Symbol&quot;; color: rgb(0, 0, 0);"><span style="font-family: &quot;Comic Sans MS&quot;;">1. Test</span></h6><h6 style="font-family: &quot;Source Sans Pro&quot;, -apple-system, BlinkMacSystemFont, &quot;Segoe UI&quot;, Roboto, &quot;Helvetica Neue&quot;, Arial, sans-serif, &quot;Apple Color Emoji&quot;, &quot;Segoe UI Emoji&quot;, &quot;Segoe UI Symbol&quot;; color: rgb(0, 0, 0);"><span style="font-family: &quot;Comic Sans MS&quot;;">2. test 3</span></h6>', CAST(N'2020-06-24T22:45:03.000' AS DateTime), 100001, 1)
SET IDENTITY_INSERT [dbo].[notifications_general] OFF
SET IDENTITY_INSERT [dbo].[owner_home] ON 

INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (1, 100001, 0)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (15, 1000004, 0)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (16, 1000005, 0)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (18, 1000006, 11009)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (17, 1000006, 11010)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (22, 1000007, 11007)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (19, 1000007, 11010)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (20, 1000008, 11009)
INSERT [dbo].[owner_home] ([id], [idUser], [idHome]) VALUES (21, 1000009, 11009)
SET IDENTITY_INSERT [dbo].[owner_home] OFF
INSERT [dbo].[position] ([idStaff], [name_position], [idLevel]) VALUES (0, N'Khách hàng', 0)
INSERT [dbo].[position] ([idStaff], [name_position], [idLevel]) VALUES (1, N'Nhân viên kinh doanh', 1)
INSERT [dbo].[position] ([idStaff], [name_position], [idLevel]) VALUES (2, N'Nhân viên quản lý', 1)
INSERT [dbo].[position] ([idStaff], [name_position], [idLevel]) VALUES (3, N'Admin', 2)
SET IDENTITY_INSERT [dbo].[services] ON 

INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (100000, CAST(N'2020-06-26T22:15:19.000' AS DateTime), CAST(N'2020-07-26T22:15:19.000' AS DateTime), 1, 11010, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (100001, CAST(N'2020-05-19T22:15:19.000' AS DateTime), CAST(N'2020-06-27T22:15:19.000' AS DateTime), 2, 11010, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (100002, CAST(N'2020-07-18T23:32:32.000' AS DateTime), CAST(N'2020-08-17T23:32:32.000' AS DateTime), 3, 11010, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (100005, CAST(N'2020-05-19T23:53:11.000' AS DateTime), CAST(N'2020-06-18T23:53:11.000' AS DateTime), 4, 11010, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000000, CAST(N'2020-06-04T01:05:01.000' AS DateTime), CAST(N'2020-07-04T01:05:01.000' AS DateTime), 1, 11009, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000001, CAST(N'2020-06-04T01:05:01.000' AS DateTime), CAST(N'2020-07-04T01:05:01.000' AS DateTime), 2, 11009, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000002, CAST(N'2020-06-04T01:05:25.000' AS DateTime), CAST(N'2020-06-10T01:05:25.000' AS DateTime), 1, 1000000, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000003, CAST(N'2020-06-04T01:05:25.000' AS DateTime), CAST(N'2020-07-04T01:05:25.000' AS DateTime), 2, 1000000, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000004, CAST(N'2020-06-04T01:05:47.000' AS DateTime), CAST(N'2020-07-04T01:05:47.000' AS DateTime), 1, 1000001, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000005, CAST(N'2020-06-04T01:05:47.000' AS DateTime), CAST(N'2020-06-10T01:05:47.000' AS DateTime), 2, 1000001, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000006, CAST(N'2020-06-04T01:06:14.000' AS DateTime), CAST(N'2020-07-04T01:06:14.000' AS DateTime), 1, 1000002, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000007, CAST(N'2020-06-04T01:06:14.000' AS DateTime), CAST(N'2020-07-04T01:06:14.000' AS DateTime), 2, 1000002, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000008, CAST(N'2020-06-04T01:06:42.000' AS DateTime), CAST(N'2020-07-04T01:06:42.000' AS DateTime), 1, 1000004, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000009, CAST(N'2020-06-04T01:06:42.000' AS DateTime), CAST(N'2020-10-09T01:06:42.000' AS DateTime), 2, 1000004, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000010, CAST(N'2020-06-04T01:06:52.000' AS DateTime), CAST(N'2020-07-04T01:06:52.000' AS DateTime), 1, 1000005, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000011, CAST(N'2020-06-04T01:06:52.000' AS DateTime), CAST(N'2020-07-04T01:06:52.000' AS DateTime), 2, 1000005, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000012, CAST(N'2020-06-04T01:07:13.000' AS DateTime), CAST(N'2020-07-04T01:07:13.000' AS DateTime), 1, 1000006, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000013, CAST(N'2020-06-04T01:07:13.000' AS DateTime), CAST(N'2020-07-04T01:07:13.000' AS DateTime), 2, 1000006, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000014, CAST(N'2020-05-28T23:32:35.000' AS DateTime), CAST(N'2020-06-28T23:32:35.000' AS DateTime), 1, 11007, 0)
INSERT [dbo].[services] ([idService], [RegDate], [ExpDate], [serviceType], [idHome], [value]) VALUES (1000015, CAST(N'2020-06-24T23:32:35.000' AS DateTime), CAST(N'2020-07-24T23:32:35.000' AS DateTime), 2, 11007, 0)
SET IDENTITY_INSERT [dbo].[services] OFF
SET IDENTITY_INSERT [dbo].[services_type] ON 

INSERT [dbo].[services_type] ([serviceType], [name_service], [description], [Price], [type], [DonVi]) VALUES (1, N'Dịch vụ Điện', N'', CAST(3000.0000 AS Decimal(19, 4)), 1, N'kWh')
INSERT [dbo].[services_type] ([serviceType], [name_service], [description], [Price], [type], [DonVi]) VALUES (2, N'Dịch vụ Nước', N'', CAST(4000.0000 AS Decimal(19, 4)), 1, N'm3')
INSERT [dbo].[services_type] ([serviceType], [name_service], [description], [Price], [type], [DonVi]) VALUES (3, N'Dịch vụ Vệ sinh', NULL, CAST(35000.0000 AS Decimal(19, 4)), 0, NULL)
INSERT [dbo].[services_type] ([serviceType], [name_service], [description], [Price], [type], [DonVi]) VALUES (4, N'Dịch vụ Internet', NULL, CAST(200000.0000 AS Decimal(19, 4)), 0, NULL)
INSERT [dbo].[services_type] ([serviceType], [name_service], [description], [Price], [type], [DonVi]) VALUES (5, N'Gửi xe', N'Gửi xe cho cư dân', CAST(50000.0000 AS Decimal(19, 4)), 0, N'')
SET IDENTITY_INSERT [dbo].[services_type] OFF
SET IDENTITY_INSERT [dbo].[statusGeneral] ON 

INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (1, N'Đang hoạt động', 1)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (2, N'Không hoạt động', 0)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (3, N'Cho thuê', 1)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (4, N'Nhà trống', 0)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (5, N'Đang sửa chữa', 0)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (6, N'Mới tạo', 2)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (7, N'Đang xử lý', 2)
INSERT [dbo].[statusGeneral] ([idStatus], [nameStatus], [idGroup]) VALUES (8, N'Đã hoàn thành', 2)
SET IDENTITY_INSERT [dbo].[statusGeneral] OFF
SET IDENTITY_INSERT [dbo].[tickets] ON 

INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (10010, 1, CAST(N'2020-05-23T23:11:49.000' AS DateTime), 8, N'Thanh toán căn hộ', 100001, 100001, 1)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (10011, 2, CAST(N'2020-05-23T23:13:45.000' AS DateTime), 6, N'Thanh toán dịch vụ', 100002, 100001, 0)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (10012, 1, CAST(N'2020-05-27T22:50:37.000' AS DateTime), 8, N'Thanh toán căn hộ', 100005, 100001, 1)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (10013, 1, CAST(N'2020-05-31T16:02:18.000' AS DateTime), 7, N'Thanh toán căn hộ', 100005, 100005, 0)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (10014, 2, CAST(N'2020-05-31T16:02:38.000' AS DateTime), 6, N'Căn hộ bị hư', 100005, 100005, 0)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (1000000, 1, CAST(N'2020-06-08T21:52:02.000' AS DateTime), 7, N'Hello Test', 1000006, 1000006, 0)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (1000001, 2, CAST(N'2020-06-08T21:52:32.000' AS DateTime), 8, N'Thanh toán căn hộ Test', 1000006, 1000006, 1)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (1000002, 2, CAST(N'2020-06-24T23:39:05.000' AS DateTime), 7, N'Quản lý test 1', 1000006, 100001, 0)
INSERT [dbo].[tickets] ([idTicket], [typeTicket], [DateCreate], [idtStatus], [TittleTicket], [idUserCreate], [idUsetLastPost], [Closed]) VALUES (1000003, 2, CAST(N'2020-06-24T23:39:15.000' AS DateTime), 6, N'Quản lý test 2', 1000006, 1000006, 0)
SET IDENTITY_INSERT [dbo].[tickets] OFF
SET IDENTITY_INSERT [dbo].[tickets_text] ON 

INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (18, 100002, CAST(N'2020-05-23T23:13:45.000' AS DateTime), N'<p>Hello</p>', 10011)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (21, 100002, CAST(N'2020-05-23T23:18:26.000' AS DateTime), N'<p>Ey</p>', 10011)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (24, 100001, CAST(N'2020-05-24T20:32:08.000' AS DateTime), N'<p>Hi các bạn</p>', 10010)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (25, 100001, CAST(N'2020-05-24T20:37:08.000' AS DateTime), N'<p><img class="img-fluid" src="https://pbs.twimg.com/profile_images/1245406088209534976/q2p0Scqf_400x400.jpg" style="width: 400px;">&nbsp;<img class="img-fluid" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTSQOWCkRSJNSdfLOjy2nHqDh46VdNqVaNUBPzPszX-J0YfCN4w&amp;s" style="width: 241px;"><br></p>', 10011)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (26, 100005, CAST(N'2020-05-27T22:50:37.000' AS DateTime), N'<p>Hj</p>', 10012)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (27, 100001, CAST(N'2020-05-30T01:41:18.000' AS DateTime), N'<p>hi cc</p>', 10012)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (28, 100005, CAST(N'2020-05-31T16:02:18.000' AS DateTime), N'<p>Test</p>', 10013)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (29, 100005, CAST(N'2020-05-31T16:02:38.000' AS DateTime), N'<p>Test</p>', 10014)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (30, 1000006, CAST(N'2020-06-08T21:52:02.000' AS DateTime), N'<p>Test</p>', 1000000)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (31, 1000006, CAST(N'2020-06-08T21:52:32.000' AS DateTime), N'<p>123</p>', 1000001)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (32, 1000006, CAST(N'2020-06-24T23:39:05.000' AS DateTime), N'<p>Quản lý test 1<br></p>', 1000002)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (33, 1000006, CAST(N'2020-06-24T23:39:15.000' AS DateTime), N'<p>Quản lý test 2<br></p>', 1000003)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (34, 1000006, CAST(N'2020-06-24T23:39:20.000' AS DateTime), N'<p>ax</p>', 1000003)
INSERT [dbo].[tickets_text] ([idtText], [idUser_post], [DateSent], [tText], [idTicket]) VALUES (35, 100001, CAST(N'2020-06-24T23:39:41.000' AS DateTime), N'<p>test</p>', 1000002)
SET IDENTITY_INSERT [dbo].[tickets_text] OFF
/****** Object:  Index [UQ__log_reas__B3FCDA312D951A52]    Script Date: 7/2/2020 8:46:33 PM ******/
ALTER TABLE [dbo].[log_reasonCancel] ADD  CONSTRAINT [UQ__log_reas__B3FCDA312D951A52] UNIQUE NONCLUSTERED 
(
	[idLog_Service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__login__3717C983887C49EC]    Script Date: 7/2/2020 8:46:33 PM ******/
ALTER TABLE [dbo].[login] ADD  CONSTRAINT [UQ__login__3717C983887C49EC] UNIQUE NONCLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__owner_ho__D0678C48E17D7629]    Script Date: 7/2/2020 8:46:33 PM ******/
ALTER TABLE [dbo].[owner_home] ADD UNIQUE NONCLUSTERED 
(
	[idUser] ASC,
	[idHome] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[accounts] ADD  CONSTRAINT [DF_accounts_idStaff]  DEFAULT ((0)) FOR [idStaff]
GO
ALTER TABLE [dbo].[apartments] ADD  CONSTRAINT [DF_apartments_idSub]  DEFAULT ((0)) FOR [idSub]
GO
ALTER TABLE [dbo].[apartments] ADD  CONSTRAINT [DF_apartments_DienTich]  DEFAULT ((0)) FOR [DienTich]
GO
ALTER TABLE [dbo].[InfoApartment] ADD  CONSTRAINT [DF_InfoApartment_id]  DEFAULT ((0)) FOR [id]
GO
ALTER TABLE [dbo].[services] ADD  CONSTRAINT [DF_services_Price]  DEFAULT ((0)) FOR [value]
GO
ALTER TABLE [dbo].[services_type] ADD  CONSTRAINT [DF_services_type_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[services_type] ADD  CONSTRAINT [DF_services_type_type]  DEFAULT ((0)) FOR [type]
GO
ALTER TABLE [dbo].[tickets] ADD  CONSTRAINT [DF_tickets_Closed]  DEFAULT ((0)) FOR [Closed]
GO
ALTER TABLE [dbo].[accounts]  WITH CHECK ADD  CONSTRAINT [get_idStaff] FOREIGN KEY([idStaff])
REFERENCES [dbo].[position] ([idStaff])
GO
ALTER TABLE [dbo].[accounts] CHECK CONSTRAINT [get_idStaff]
GO
ALTER TABLE [dbo].[apartments]  WITH CHECK ADD  CONSTRAINT [get_idStatusGeneral] FOREIGN KEY([statusGeneral])
REFERENCES [dbo].[statusGeneral] ([idStatus])
GO
ALTER TABLE [dbo].[apartments] CHECK CONSTRAINT [get_idStatusGeneral]
GO
ALTER TABLE [dbo].[apartments]  WITH CHECK ADD  CONSTRAINT [get_idType_apartments] FOREIGN KEY([typeApartment])
REFERENCES [dbo].[apartments_type] ([idType])
GO
ALTER TABLE [dbo].[apartments] CHECK CONSTRAINT [get_idType_apartments]
GO
ALTER TABLE [dbo].[login]  WITH CHECK ADD  CONSTRAINT [FK_login_accounts] FOREIGN KEY([idUser])
REFERENCES [dbo].[accounts] ([idUser])
GO
ALTER TABLE [dbo].[login] CHECK CONSTRAINT [FK_login_accounts]
GO
ALTER TABLE [dbo].[notifications_general]  WITH CHECK ADD  CONSTRAINT [get_typeUser] FOREIGN KEY([typeUser])
REFERENCES [dbo].[position] ([idStaff])
GO
ALTER TABLE [dbo].[notifications_general] CHECK CONSTRAINT [get_typeUser]
GO
ALTER TABLE [dbo].[owner_home]  WITH CHECK ADD  CONSTRAINT [FK_owner_home_accounts] FOREIGN KEY([idUser])
REFERENCES [dbo].[accounts] ([idUser])
GO
ALTER TABLE [dbo].[owner_home] CHECK CONSTRAINT [FK_owner_home_accounts]
GO
ALTER TABLE [dbo].[services]  WITH CHECK ADD  CONSTRAINT [get_idHome] FOREIGN KEY([idHome])
REFERENCES [dbo].[apartments] ([idMain])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[services] CHECK CONSTRAINT [get_idHome]
GO
ALTER TABLE [dbo].[services]  WITH CHECK ADD  CONSTRAINT [get_serviceType] FOREIGN KEY([serviceType])
REFERENCES [dbo].[services_type] ([serviceType])
GO
ALTER TABLE [dbo].[services] CHECK CONSTRAINT [get_serviceType]
GO
ALTER TABLE [dbo].[tickets]  WITH CHECK ADD  CONSTRAINT [get_idtStatus] FOREIGN KEY([idtStatus])
REFERENCES [dbo].[statusGeneral] ([idStatus])
GO
ALTER TABLE [dbo].[tickets] CHECK CONSTRAINT [get_idtStatus]
GO
ALTER TABLE [dbo].[tickets]  WITH CHECK ADD  CONSTRAINT [get_typeTicket] FOREIGN KEY([typeTicket])
REFERENCES [dbo].[position] ([idStaff])
GO
ALTER TABLE [dbo].[tickets] CHECK CONSTRAINT [get_typeTicket]
GO
ALTER TABLE [dbo].[tickets_text]  WITH CHECK ADD  CONSTRAINT [get_idTicket] FOREIGN KEY([idTicket])
REFERENCES [dbo].[tickets] ([idTicket])
GO
ALTER TABLE [dbo].[tickets_text] CHECK CONSTRAINT [get_idTicket]
GO
USE [master]
GO
ALTER DATABASE [apartment] SET  READ_WRITE 
GO
