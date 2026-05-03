IF COL_LENGTH('Orders', 'Message') IS NOT NULL
BEGIN
    ALTER TABLE Orders DROP COLUMN Message;
END

IF COL_LENGTH('Orders', 'Massage') IS NOT NULL
BEGIN
    ALTER TABLE Orders ALTER COLUMN Massage nvarchar(max) NOT NULL;
END

IF COL_LENGTH('Orders', 'ReservationDate') IS NULL
BEGIN
    ALTER TABLE Orders ADD ReservationDate datetime2 NOT NULL DEFAULT GETDATE();
END

IF COL_LENGTH('Orders', 'ReservationTime') IS NULL
BEGIN
    ALTER TABLE Orders ADD ReservationTime nvarchar(max) NOT NULL DEFAULT '09:00';
END
UPDATE Orders
SET ReservationDate = GETDATE()
WHERE ReservationDate IS NULL;

UPDATE Orders
SET ReservationTime = '09:00'
WHERE ReservationTime IS NULL;

UPDATE Orders
SET Massage = 'Няма съобщение'
WHERE Massage IS NULL;
ALTER TABLE Orders
ADD CONSTRAINT DF_Orders_ReservationDate DEFAULT GETDATE() FOR ReservationDate;

ALTER TABLE Orders
ADD CONSTRAINT DF_Orders_ReservationTime DEFAULT '09:00' FOR ReservationTime;

ALTER TABLE Orders
ADD CONSTRAINT DF_Orders_Massage DEFAULT 'Няма съобщение' FOR Massage;