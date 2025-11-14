USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'DB_RoomReservation')
BEGIN
    ALTER DATABASE DB_RoomReservation SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DB_RoomReservation;
END
GO

CREATE DATABASE DB_RoomReservation;
GO

USE DB_RoomReservation;
GO

CREATE TABLE Rooms (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoomName NVARCHAR(200) NOT NULL,
    RoomType NVARCHAR(100) NULL,
    Capacity INT NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

CREATE INDEX IX_Room_RoomName ON Rooms(RoomName);
GO

CREATE INDEX IX_Room_Status ON Rooms(Status);
GO

CREATE TABLE Reservations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoomName NVARCHAR(100) NOT NULL,
    CustomerName NVARCHAR(100) NOT NULL,
    ContactNumber NVARCHAR(20) NULL,
    Email NVARCHAR(255) NULL,
    CheckInDate DATETIME2 NOT NULL,
    CheckOutDate DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

CREATE INDEX IX_Reservation_Status ON Reservations(Status);
GO

CREATE INDEX IX_Reservation_CustomerName ON Reservations(CustomerName);
GO

CREATE INDEX IX_Reservation_CheckInDate ON Reservations(CheckInDate);
GO

CREATE INDEX IX_Reservation_CheckOutDate ON Reservations(CheckOutDate);
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NULL DEFAULT 'User',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
GO

INSERT INTO Users (Email, PasswordHash, FullName, Role) VALUES
('admin@roomreservation.com', 'admin123', 'Administrator', 'Admin'),
('user@roomreservation.com', 'user123', 'Test User', 'User');
GO

INSERT INTO Rooms (RoomName, RoomType, Capacity, Status, Description) VALUES
('Conference Room A', 'Conference', 20, 'Available', 'Large conference room with projector and whiteboard'),
('Conference Room B', 'Conference', 15, 'Available', 'Medium conference room with video conferencing'),
('Meeting Room 1', 'Meeting', 8, 'Available', 'Small meeting room for team discussions'),
('Meeting Room 2', 'Meeting', 6, 'Available', 'Intimate meeting space'),
('Training Room', 'Training', 30, 'Available', 'Large training room with presentation equipment'),
('Executive Suite', 'Executive', 4, 'Available', 'Premium executive meeting room');
GO

INSERT INTO Reservations (RoomName, CustomerName, ContactNumber, Email, CheckInDate, CheckOutDate, Status, PaymentStatus) VALUES
('Conference Room A', 'John Doe', '09123456789', 'john.doe@email.com', DATEADD(day, 1, GETDATE()), DATEADD(day, 2, GETDATE()), 'Pending', 'Pending'),
('Meeting Room 1', 'Jane Smith', '09987654321', 'jane.smith@email.com', DATEADD(day, 3, GETDATE()), DATEADD(day, 3, GETDATE()), 'Confirmed', 'Paid'),
('Training Room', 'Mike Johnson', '09876543210', 'mike.johnson@email.com', DATEADD(day, 5, GETDATE()), DATEADD(day, 6, GETDATE()), 'Confirmed', 'Pending');
GO

PRINT 'Database DB_RoomReservation created successfully!';
PRINT 'Tables: Rooms, Reservations, Users';
PRINT 'Sample data inserted.';
GO

