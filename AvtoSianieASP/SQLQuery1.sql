IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = N'Детайлинг')
BEGIN
    INSERT INTO Categories (Name, DateOn)
    VALUES (N'Детайлинг', GETDATE());
END

DECLARE @CategoryId INT;

SELECT @CategoryId = Id
FROM Categories
WHERE Name = N'Детайлинг';

INSERT INTO Serveces
(KatNum, DescSurves, CategoryId, Equipment, Duration, Image, Price, DateOn)
VALUES
(N'USL-001', N'Външно измиване', @CategoryId, 1, 30, N'/images/services/ChatGPT Image May 9, 2026, 05_28_54 PM.png', 20, GETDATE()),
(N'USL-002', N'Вътрешно почистване', @CategoryId, 2, 60, N'/images/services/ChatGPT Image May 9, 2026, 05_23_43 PM.png', 40, GETDATE()),
(N'USL-003', N'Пастиране и полиране', @CategoryId, 3, 180, N'/images/services/ChatGPT Image May 9, 2026, 05_25_07 PM.png', 120, GETDATE()),
(N'USL-004', N'Пълен детайлинг', @CategoryId, 4, 480, N'/images/services/ChatGPT Image May 9, 2026, 05_27_24 PM.png', 250, GETDATE());