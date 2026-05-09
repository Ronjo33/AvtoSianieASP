IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'Детайлинг')
BEGIN
    INSERT INTO Categories (Name)
    VALUES (N'Детайлинг');
END

DECLARE @CategoryId int;
SELECT @CategoryId = Id FROM Categories WHERE Name = N'Детайлинг';

INSERT INTO Serveces (KatNum, DescSurves, CategoryId, Equipment, Duration, Image, Price, DateOn)
VALUES
(N'USL-001', N'Външно измиване', @CategoryId, N'Активна пяна, шампоан, микрофибър', N'30 мин', N'/images/homeimg.jpg', 20, GETDATE()),

(N'USL-002', N'Вътрешно почистване', @CategoryId, N'Прахосмукачка, препарати, четки', N'60 мин', N'/images/homeimg.jpg', 40, GETDATE()),

(N'USL-003', N'Пастиране и полиране', @CategoryId, N'Полир машина, пасти, падове', N'3 часа', N'/images/homeimg.jpg', 120, GETDATE()),

(N'USL-004', N'Пълно детайлинг обслужване', @CategoryId, N'Пране, полиране, защита', N'1 ден', N'/images/homeimg.jpg', 250, GETDATE());