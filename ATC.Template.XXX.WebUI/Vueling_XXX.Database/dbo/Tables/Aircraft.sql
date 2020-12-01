CREATE TABLE [dbo].[Aircraft] (
    [FlightNumber]  VARCHAR (50)  NOT NULL,
    [DepartureDate] DATETIME      NOT NULL,
    [BusySeats]     VARCHAR (MAX) NULL,
    [Seats]         VARCHAR (MAX) NOT NULL,
    [LastUpdate]    DATETIME      NOT NULL,
    CONSTRAINT [PK_Aircraft] PRIMARY KEY CLUSTERED ([FlightNumber] ASC, [DepartureDate] ASC)
);

