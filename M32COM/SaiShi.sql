USE [SaiShi]
GO
/****** Object:  Table [dbo].[SaiShi_tDepartment]    Script Date: 2019/3/2 21:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaiShi_tDepartment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SaiShiID] [int] NULL,
	[Name] [nvarchar](550) NULL,
	[Score] [decimal](18, 2) NULL,
	[CreateTime] [datetime] NULL,
	[CreateID] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_Finance_tDepartment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaiShi_tPinFen]    Script Date: 2019/3/2 21:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaiShi_tPinFen](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SaiShiID] [int] NULL,
	[DepartmentID] [int] NULL,
	[UserID] [int] NULL,
	[Score] [decimal](18, 2) NULL,
	[CreateTime] [datetime] NULL,
	[CreateID] [int] NULL,
	[Status] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SaiShi_tDeFen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaiShi_tSaiShi]    Script Date: 2019/3/2 21:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaiShi_tSaiShi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](550) NULL,
	[JianJie] [nvarchar](max) NULL,
	[PinFenGuiZe] [nvarchar](max) NULL,
	[BeginTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[CreateTime] [datetime] NULL,
	[CreateID] [int] NULL,
	[Status] [int] NULL,
	[img1] [nvarchar](550) NULL,
	[img2] [nvarchar](550) NULL,
	[img3] [nvarchar](550) NULL,
	[img4] [nvarchar](550) NULL,
	[img5] [nvarchar](550) NULL,
	[Remark] [ntext] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SaiShi_tBanJI] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaiShi_tUser]    Script Date: 2019/3/2 21:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaiShi_tUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [int] NULL,
	[UserName] [nvarchar](550) NULL,
	[UserPassword] [nvarchar](550) NULL,
	[TrueName] [nvarchar](550) NULL,
	[SaiShiID] [int] NULL,
	[DepartmentID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[CreateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[Remark] [ntext] NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](150) NULL,
	[Age] [nvarchar](10) NULL,
	[JianJie] [nvarchar](20) NULL,
	[Status] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_Finance_tUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[SaiShi_tDepartment] ON 

INSERT [dbo].[SaiShi_tDepartment] ([ID], [SaiShiID], [Name], [Score], [CreateTime], [CreateID], [IsDelete]) VALUES (1, 1, N'A组', CAST(89.03 AS Decimal(18, 2)), CAST(N'2019-03-01 22:18:57.107' AS DateTime), 1, 0)
INSERT [dbo].[SaiShi_tDepartment] ([ID], [SaiShiID], [Name], [Score], [CreateTime], [CreateID], [IsDelete]) VALUES (2, 1, N'B组', CAST(76.23 AS Decimal(18, 2)), CAST(N'2019-03-02 11:00:36.013' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[SaiShi_tDepartment] OFF
SET IDENTITY_INSERT [dbo].[SaiShi_tSaiShi] ON 

INSERT [dbo].[SaiShi_tSaiShi] ([ID], [Name], [JianJie], [PinFenGuiZe], [BeginTime], [EndTime], [CreateTime], [CreateID], [Status], [img1], [img2], [img3], [img4], [img5], [Remark], [IsDelete]) VALUES (1, N'2019年3月份皮划艇4人赛事', N'<p>东方红说的很对</p><p>然而无若</p><p>第三个地方好大航海家</p><p>爽肤水干啥的嘎多个</p>', N'<p>efgwteyt</p><p>而他认为特特温特我</p>', CAST(N'2019-03-01 00:00:00.000' AS DateTime), CAST(N'2019-03-04 00:00:00.000' AS DateTime), CAST(N'2019-03-01 22:17:55.423' AS DateTime), 1, 2, N'/UpLoad//UpLoad/2019_03_02/14105_2019_03_02_10_41_36_366.jpg/56542_2019_03_02_10_51_07_087.jpg', N'/UpLoad/2019_03_02/81369_2019_03_02_10_41_40_509.jpg', N'/UpLoad/2019_03_02/63319_2019_03_02_10_41_44_856.jpg', N'/UpLoad/2019_03_02/29125_2019_03_02_10_41_49_815.jpg', N'/UpLoad/2019_03_02/45094_2019_03_02_10_41_53_964.jpg', NULL, 0)
INSERT [dbo].[SaiShi_tSaiShi] ([ID], [Name], [JianJie], [PinFenGuiZe], [BeginTime], [EndTime], [CreateTime], [CreateID], [Status], [img1], [img2], [img3], [img4], [img5], [Remark], [IsDelete]) VALUES (2, N'2019年游泳比赛1', N'<p style="margin-top: 0px; margin-bottom: 18px; padding: 0px; color: rgb(77, 79, 83); font-family: &#39;Microsoft Yahei&#39;, &#39;FAE软雅黑&#39;, &#39;STHeiti Light&#39;, &#39;+4E文细黑&#39;, SimSun, &#39;B8B体&#39;, Arial, sans-serif; font-size: 18px; letter-spacing: 1px; line-height: 32px; white-space: normal; background-color: rgb(255, 255, 255);">3月1日，战机被巴基斯坦军方击落，自己也被巴方俘虏的印度空军中校阿比纳丹，终于被巴基斯坦方面交还给了印度。接下来他将搭乘飞机前往印度德里，与家人团聚。</p><p style="margin-top: 0px; margin-bottom: 18px; padding: 0px; color: rgb(77, 79, 83); font-family: &#39;Microsoft Yahei&#39;, &#39;FAE软雅黑&#39;, &#39;STHeiti Light&#39;, &#39;+4E文细黑&#39;, SimSun, &#39;B8B体&#39;, Arial, sans-serif; font-size: 18px; letter-spacing: 1px; line-height: 32px; white-space: normal; background-color: rgb(255, 255, 255);">　　而印度媒体和民众此刻也陷入了“全国狂欢”的状态，庆祝这位“传奇英雄”的归来…..</p><p><br/></p>', N'<p><span style="color: rgb(77, 79, 83); font-family: &#39;Microsoft Yahei&#39;, &#39;FAE软雅黑&#39;, &#39;STHeiti Light&#39;, &#39;+4E文细黑&#39;, SimSun, &#39;B8B体&#39;, Arial, sans-serif; font-size: 18px; letter-spacing: 1px; line-height: 32px; background-color: rgb(255, 255, 255);">然而，今天印度人等待阿比纳丹被释放的过程，却极为漫长。从印度媒体的直播来看，阿比纳丹的释放时间原本是印度时间上午10点左右(北京时间12：30)，之后又被安排到在印度时间下午2点的，但由于外交手续和医疗检查等种种因素，最终他被释放回印度的时间为印度时间晚上9点左右，几乎把一整天都搭进去了。</span></p>', CAST(N'2019-03-13 00:00:00.000' AS DateTime), CAST(N'2019-03-18 00:00:00.000' AS DateTime), CAST(N'2019-03-02 15:13:21.347' AS DateTime), 1, 0, N'/UpLoad/2019_03_02/80707_2019_03_02_15_12_55_182.jpg', N'/UpLoad/2019_03_02/80539_2019_03_02_15_13_01_144.jpg', N'/UpLoad/2019_03_02/11798_2019_03_02_15_13_06_931.jpg', N'/UpLoad/2019_03_02/59878_2019_03_02_15_13_14_648.jpg', N'/UpLoad/2019_03_02/27083_2019_03_02_15_13_20_064.jpg', NULL, 0)
SET IDENTITY_INSERT [dbo].[SaiShi_tSaiShi] OFF
SET IDENTITY_INSERT [dbo].[SaiShi_tUser] ON 

INSERT [dbo].[SaiShi_tUser] ([ID], [UserType], [UserName], [UserPassword], [TrueName], [SaiShiID], [DepartmentID], [CreateTime], [CreateID], [UpdateTime], [UpdateID], [Remark], [Phone], [Email], [Age], [JianJie], [Status], [IsDelete]) VALUES (1, 0, N'admin', N'4QrcOUm6Wau+VuBX8g+IPg==', N'超级管理员', NULL, 0, CAST(N'1900-01-01 00:00:00.000' AS DateTime), 0, CAST(N'2017-10-11 09:43:00.000' AS DateTime), 0, NULL, NULL, NULL, NULL, NULL, 0, 0)
INSERT [dbo].[SaiShi_tUser] ([ID], [UserType], [UserName], [UserPassword], [TrueName], [SaiShiID], [DepartmentID], [CreateTime], [CreateID], [UpdateTime], [UpdateID], [Remark], [Phone], [Email], [Age], [JianJie], [Status], [IsDelete]) VALUES (2, 2, N'dyjh', N'4QrcOUm6Wau+VuBX8g+IPg==', N'刘双喜', 1, 1, CAST(N'2019-03-01 22:26:13.943' AS DateTime), 1, CAST(N'2019-03-02 17:40:08.500' AS DateTime), 1, NULL, N'13456789023', N'235343@qq.com', N'23', NULL, 0, 0)
INSERT [dbo].[SaiShi_tUser] ([ID], [UserType], [UserName], [UserPassword], [TrueName], [SaiShiID], [DepartmentID], [CreateTime], [CreateID], [UpdateTime], [UpdateID], [Remark], [Phone], [Email], [Age], [JianJie], [Status], [IsDelete]) VALUES (3, 1, N'test', N'4QrcOUm6Wau+VuBX8g+IPg==', N'张之洞', 0, 0, CAST(N'2019-03-01 22:27:15.320' AS DateTime), 1, CAST(N'1990-01-01 00:00:00.000' AS DateTime), 0, NULL, N'13456789089', NULL, N'21', NULL, 0, 0)
INSERT [dbo].[SaiShi_tUser] ([ID], [UserType], [UserName], [UserPassword], [TrueName], [SaiShiID], [DepartmentID], [CreateTime], [CreateID], [UpdateTime], [UpdateID], [Remark], [Phone], [Email], [Age], [JianJie], [Status], [IsDelete]) VALUES (4, 1, N'drtx2016', N'4QrcOUm6Wau+VuBX8g+IPg==', N'张之洞', 0, 0, CAST(N'2019-03-01 22:29:05.543' AS DateTime), 1, CAST(N'1990-01-01 00:00:00.000' AS DateTime), 0, NULL, N'13456789078', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[SaiShi_tUser] ([ID], [UserType], [UserName], [UserPassword], [TrueName], [SaiShiID], [DepartmentID], [CreateTime], [CreateID], [UpdateTime], [UpdateID], [Remark], [Phone], [Email], [Age], [JianJie], [Status], [IsDelete]) VALUES (5, 1, N'asfdfsdfs', N'M1jprfFrZcIhsyAO+Ex/hA==', N'24242', 0, 0, CAST(N'2019-03-02 17:29:13.033' AS DateTime), 1, CAST(N'1990-01-01 00:00:00.000' AS DateTime), 0, NULL, N'13456789034', NULL, NULL, NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[SaiShi_tUser] OFF
ALTER TABLE [dbo].[SaiShi_tPinFen] ADD  CONSTRAINT [DF_SaiShi_tPinFen_Status]  DEFAULT ((0)) FOR [Status]
GO
