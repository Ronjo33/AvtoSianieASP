DECLARE @CategoryId int;

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'Детайлинг')
BEGIN
    INSERT INTO Categories (Name, DateOn)
    VALUES (N'Детайлинг', GETDATE());
END

SELECT @CategoryId = Id
FROM Categories
WHERE Name = N'Детайлинг';

INSERT INTO Serveces
(KatNum, DescSurves, CategoryId, Equipment, Duration, Image, Price, DateOn)
VALUES
(N'USL-001', N'Външно измиване', @CategoryId, 1, 30, N'/images/homeimg.jpg', 20, GETDATE()),

(N'USL-002', N'Вътрешно почистване', @CategoryId, 2, 60, N'/images/homeimg.jpg', 40, GETDATE()),

(N'USL-003', N'Пастиране и полиране', @CategoryId, 3, 180, N'/images/homeimg.jpg', 120, GETDATE()),

(N'USL-004', N'Пълен детайлинг', @CategoryId, 4, 480, N'/images/homeimg.jpg', 250, GETDATE());
UPDATE AspNetUsers
SET UserName = 'Ronjo1',
    NormalizedUserName = 'RONJO1'
WHERE Email = 'zlatomirstefanov28@gmail.com';