USE [xjs]
GO

/****** Object:  Table [dbo].[TaskAutoRun]    Script Date: 2017-09-14 14:54:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskAutoRun](
	[id] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NULL,
	[path] [nvarchar](150) NULL,
	[runType] [int] NULL,
	[runMiniteOf] [int] NULL,
	[runTimeAt] [time](7) NULL,
	[Author] [nvarchar](150) NULL,
	[LastRunTime] [datetime] NULL,
	[isOpen] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

