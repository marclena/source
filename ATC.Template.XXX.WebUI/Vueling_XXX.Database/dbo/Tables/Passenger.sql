CREATE TABLE [dbo].[Passenger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[PaxType] [int] NOT NULL,
	[BookingId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Passenger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)

GO

ALTER TABLE [dbo].[Passenger]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Passenger_dbo.Booking_BookingId] FOREIGN KEY([BookingId])
REFERENCES [dbo].[Booking] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_dbo.Passenger_dbo.Booking_BookingId]
GO