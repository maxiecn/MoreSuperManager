USE [MoreSuperManager]
GO
/****** Object:  Table [dbo].[T_VoteType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_VoteType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_VoteTy__30667A257F60ED59] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_VoteText]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_VoteText](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[VoteID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[VoteText] [nvarchar](300) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_VoteItem]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_VoteItem](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [varchar](50) NOT NULL,
	[VoteID] [int] NOT NULL,
	[ItemTitle] [nvarchar](200) NOT NULL,
	[ItemType] [int] NOT NULL,
	[ItemContent] [nvarchar](100) NOT NULL,
	[ItemMaxCount] [int] NOT NULL,
	[ItemNum] [int] NOT NULL,
 CONSTRAINT [PK__T_Vote_I__30667A25797309D9] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Vote]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Vote](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[VoteType] [int] NOT NULL,
	[VoteTitle] [nvarchar](200) NOT NULL,
	[VoteSummary] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK__T_Vote__30667A25060DEAE8] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_UserLog]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_UserLog](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[UserCode] [varchar](50) NOT NULL,
	[LoginIP] [varchar](20) NOT NULL,
	[LoginDate] [datetime] NOT NULL,
	[LoginStatus] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_User]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_User](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[UserCode] [varchar](50) NOT NULL,
	[NickName] [nvarchar](30) NOT NULL,
	[UserPassword] [varchar](64) NOT NULL,
	[RoleID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_TopicType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_TopicType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[ParentID] [int] NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_TopicT__30667A25117F9D94] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_TopicPositionType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_TopicPositionType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_TopicP__30667A2515502E78] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Topic]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Topic](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TopicType] [int] NOT NULL,
	[PositionTypeList] [varchar](200) NOT NULL,
	[TopicTitle] [nvarchar](100) NOT NULL,
	[TopicTags] [nvarchar](50) NULL,
	[TopicCoverImageUrl] [varchar](200) NULL,
	[TopicSummary] [nvarchar](500) NULL,
	[TopicContent] [nvarchar](max) NOT NULL,
	[TopicOriginalWebsite] [nvarchar](50) NULL,
	[TopicOriginalUrl] [nvarchar](200) NULL,
	[TopicUserCode] [varchar](50) NOT NULL,
	[TopicStatus] [int] NOT NULL,
	[TopicVisitNum] [int] NOT NULL,
	[TopicDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK__T_Topic__30667A251920BF5C] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Role]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Role](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[MenuList] [varchar](2000) NOT NULL,
	[ActionList] [varchar](max) NOT NULL,
 CONSTRAINT [PK__T_Role__8A2B616108EA5793] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_ProjectType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_ProjectType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_Projec__30667A2560A75C0F] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Project]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Project](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[ProjectType] [int] NOT NULL,
	[ProjectName] [nvarchar](100) NOT NULL,
	[FlowID] [int] NOT NULL,
	[FlowStepID] [int] NOT NULL,
	[RoleList] [varchar](100) NOT NULL,
 CONSTRAINT [PK__T_Projec__30667A25656C112C] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_NoticeType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_NoticeType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_Notice__30667A2522AA2996] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Notice]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Notice](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[NoticeType] [int] NOT NULL,
	[NoticeTitle] [nvarchar](50) NOT NULL,
	[NoticeContent] [nvarchar](1000) NOT NULL,
	[NoticeDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK__T_Notice__30667A25267ABA7A] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Module]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Module](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleCode] [varchar](50) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[ModuleName] [nvarchar](30) NULL,
	[ActionList] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_T_Module] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_MessageReply]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_MessageReply](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NOT NULL,
	[ReplyContent] [nvarchar](max) NOT NULL,
	[UserCode] [varchar](50) NOT NULL,
	[NickName] [nvarchar](30) NOT NULL,
	[ReplyDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Message]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Message](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ContactName] [nvarchar](50) NOT NULL,
	[ContactTelphone] [varchar](20) NULL,
	[ContactEmail] [varchar](50) NULL,
	[MessageContent] [nvarchar](max) NOT NULL,
	[ContactIP] [varchar](20) NOT NULL,
	[MessageStatus] [int] NOT NULL,
	[MessageDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Menu]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Menu](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[ParentID] [int] NOT NULL,
	[MenuName] [nvarchar](50) NOT NULL,
	[MenuUrl] [nvarchar](200) NULL,
	[BelongModule] [varchar](50) NULL,
	[ActionList] [varchar](500) NULL,
	[MenuSort] [int] NOT NULL,
	[MenuIcon] [varchar](100) NULL,
 CONSTRAINT [PK__T_Menu__30667A250425A276] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_LinkFriendType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_LinkFriendType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_LinkFr__30667A2537A5467C] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_LinkFriend]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_LinkFriend](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[LinkFriendType] [int] NOT NULL,
	[LinkFriendCoverImageUrl] [nvarchar](200) NULL,
	[LinkFriendName] [nvarchar](50) NOT NULL,
	[LinkFriendUrl] [varchar](200) NOT NULL,
	[LinkFriendSort] [int] NOT NULL,
 CONSTRAINT [PK__T_LinkFr__30667A2530F848ED] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_IndexMapper]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_IndexMapper](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[IndexType] [int] NOT NULL,
	[IndexID] [int] NOT NULL,
	[MapperID] [int] NOT NULL,
 CONSTRAINT [PK__T_IndexM__30667A253D5E1FD2] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_FlowType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_FlowType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK__T_FlowTy__30667A25412EB0B6] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_FlowSymbolType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_FlowSymbolType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[TypeCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](30) NULL,
	[TypeSort] [int] NOT NULL,
 CONSTRAINT [PK_T_FlowSymbolType] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_FlowStep]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_FlowStep](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[FlowID] [int] NOT NULL,
	[StepCode] [varchar](50) NOT NULL,
	[StepSymbol] [varchar](50) NOT NULL,
	[StepName] [nvarchar](50) NOT NULL,
	[StepAddrName] [nvarchar](20) NOT NULL,
	[RoleList] [varchar](100) NOT NULL,
	[StepList] [varchar](200) NOT NULL,
	[NextStep] [varchar](50) NULL,
	[PositionTop] [int] NOT NULL,
	[PositionLeft] [int] NOT NULL,
 CONSTRAINT [PK_T_FlowStep] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Flow]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Flow](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[FlowType] [int] NOT NULL,
	[FlowName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__T_Flow__30667A254AB81AF0] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Channel]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Channel](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelCode] [varchar](50) NOT NULL,
	[ChannelName] [nvarchar](20) NOT NULL,
	[ChannelSort] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ChannelCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Attachment]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Attachment](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[AttachmentType] [varchar](20) NOT NULL,
	[AttachmentName] [nvarchar](100) NOT NULL,
	[AttachmentSize] [int] NOT NULL,
	[AttachmentPath] [varchar](200) NOT NULL,
	[AttachmentDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Application]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Application](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationIcon] [varchar](50) NULL,
	[ApplicationUrl] [nvarchar](200) NOT NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
	[ApplicationType] [varchar](20) NOT NULL,
	[ApplicationX] [int] NOT NULL,
	[ApplicationY] [int] NOT NULL,
	[ApplicationWidth] [int] NOT NULL,
	[ApplicationHeight] [int] NOT NULL,
 CONSTRAINT [PK__T_Applic__30667A256BE40491] PRIMARY KEY CLUSTERED 
(
	[IdentityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_ActionType]    Script Date: 03/06/2019 16:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_ActionType](
	[IdentityID] [int] IDENTITY(1,1) NOT NULL,
	[TypeCode] [varchar](50) NOT NULL,
	[TypeName] [nvarchar](30) NULL,
	[TypeSort] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF__T_ActionT__TypeS__5629CD9C]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_ActionType] ADD  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_Attachm__Attac__571DF1D5]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Attachment] ADD  DEFAULT (getdate()) FOR [AttachmentDate]
GO
/****** Object:  Default [DF__T_Channel__Chann__03F0984C]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Channel] ADD  DEFAULT ((0)) FOR [ChannelSort]
GO
/****** Object:  Default [DF_T_Flow_Step_Top]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_FlowStep] ADD  CONSTRAINT [DF_T_Flow_Step_Top]  DEFAULT ((0)) FOR [PositionTop]
GO
/****** Object:  Default [DF_T_Flow_Step_Left]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_FlowStep] ADD  CONSTRAINT [DF_T_Flow_Step_Left]  DEFAULT ((0)) FOR [PositionLeft]
GO
/****** Object:  Default [DF__T_FlowSym__TypeS__59FA5E80]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_FlowSymbolType] ADD  CONSTRAINT [DF__T_FlowSym__TypeS__59FA5E80]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_FlowTyp__TypeS__5AEE82B9]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_FlowType] ADD  CONSTRAINT [DF__T_FlowTyp__TypeS__5AEE82B9]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_LinkFri__LinkF__32E0915F]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_LinkFriend] ADD  CONSTRAINT [DF__T_LinkFri__LinkF__32E0915F]  DEFAULT ((0)) FOR [LinkFriendSort]
GO
/****** Object:  Default [DF__T_LinkFri__TypeS__5CD6CB2B]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_LinkFriendType] ADD  CONSTRAINT [DF__T_LinkFri__TypeS__5CD6CB2B]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_Menu__MenuSort__060DEAE8]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Menu] ADD  CONSTRAINT [DF__T_Menu__MenuSort__060DEAE8]  DEFAULT ((0)) FOR [MenuSort]
GO
/****** Object:  Default [DF__T_Message__Messa__5FB337D6]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Message] ADD  DEFAULT ((0)) FOR [MessageStatus]
GO
/****** Object:  Default [DF__T_Message__Messa__60A75C0F]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Message] ADD  DEFAULT (getdate()) FOR [MessageDate]
GO
/****** Object:  Default [DF__T_Message__Reply__619B8048]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_MessageReply] ADD  DEFAULT (getdate()) FOR [ReplyDate]
GO
/****** Object:  Default [DF__T_Notice__Notice__628FA481]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Notice] ADD  CONSTRAINT [DF__T_Notice__Notice__628FA481]  DEFAULT (getdate()) FOR [NoticeDateTime]
GO
/****** Object:  Default [DF__T_NoticeT__TypeS__6383C8BA]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_NoticeType] ADD  CONSTRAINT [DF__T_NoticeT__TypeS__6383C8BA]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_Project__FlowN__6754599E]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Project] ADD  CONSTRAINT [DF__T_Project__FlowN__6754599E]  DEFAULT ((0)) FOR [FlowStepID]
GO
/****** Object:  Default [DF_T_Project_RoleList]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Project] ADD  CONSTRAINT [DF_T_Project_RoleList]  DEFAULT ('') FOR [RoleList]
GO
/****** Object:  Default [DF__T_Project__TypeS__628FA481]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_ProjectType] ADD  CONSTRAINT [DF__T_Project__TypeS__628FA481]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_Topic__TopicSt__66603565]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Topic] ADD  CONSTRAINT [DF__T_Topic__TopicSt__66603565]  DEFAULT ((0)) FOR [TopicStatus]
GO
/****** Object:  Default [DF__T_Topic__TopicVi__6754599E]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Topic] ADD  CONSTRAINT [DF__T_Topic__TopicVi__6754599E]  DEFAULT ((0)) FOR [TopicVisitNum]
GO
/****** Object:  Default [DF__T_Topic__TopicDa__68487DD7]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_Topic] ADD  CONSTRAINT [DF__T_Topic__TopicDa__68487DD7]  DEFAULT (getdate()) FOR [TopicDateTime]
GO
/****** Object:  Default [DF__T_TopicPo__TypeS__693CA210]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_TopicPositionType] ADD  CONSTRAINT [DF__T_TopicPo__TypeS__693CA210]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_TopicTy__TypeS__6A30C649]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_TopicType] ADD  CONSTRAINT [DF__T_TopicTy__TypeS__6A30C649]  DEFAULT ((0)) FOR [TypeSort]
GO
/****** Object:  Default [DF__T_UserLog__Login__6B24EA82]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_UserLog] ADD  DEFAULT (getdate()) FOR [LoginDate]
GO
/****** Object:  Default [DF__T_UserLog__Login__6C190EBB]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_UserLog] ADD  DEFAULT ((0)) FOR [LoginStatus]
GO
/****** Object:  Default [DF__T_Vote_It__ItemN__7B5B524B]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_VoteItem] ADD  CONSTRAINT [DF__T_Vote_It__ItemN__7B5B524B]  DEFAULT ((0)) FOR [ItemNum]
GO
/****** Object:  Default [DF__T_VoteTyp__TypeS__6E01572D]    Script Date: 03/06/2019 16:16:44 ******/
ALTER TABLE [dbo].[T_VoteType] ADD  CONSTRAINT [DF__T_VoteTyp__TypeS__6E01572D]  DEFAULT ((0)) FOR [TypeSort]
GO
