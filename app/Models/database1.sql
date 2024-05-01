USE master
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'database1')
   CREATE DATABASE database1
GO


USE database1

IF OBJECT_ID('database1..Table2', 'U') IS NOT NULL
  DROP TABLE database1..Table2
GO

IF OBJECT_ID('database1..Table1', 'U') IS NOT NULL
  DROP TABLE database1..Table1
GO

CREATE TABLE database1..Table1
(
  UUID INT IDENTITY NOT NULL PRIMARY KEY,
  Number1 INT NOT NULL,
  Text1 NVARCHAR(50) NOT NULL  
);
GO

INSERT INTO database1..Table1 ([Number1], [Text1] ) VALUES
(1, 'One'),
(2, 'Two'),
(3, 'Three'),
(4, 'Four'),
(5, 'Five'),
(6, 'Six'),
(7, 'Seven'),
(8, 'Eight'),
(9, 'Nine'),
(10, 'Ten');
GO

UPDATE database1..Table1 SET [Number1] = 3, [Text1] = 'Three' WHERE UUID = 2
GO

SELECT * FROM database1..Table1;
GO

CREATE TABLE database1..Table2
(
  UUID INT IDENTITY NOT NULL PRIMARY KEY,
  Number2 INT NOT NULL,
  Text2 NVARCHAR(50) NOT NULL 
  CONSTRAINT fk_UUID FOREIGN KEY (Number2) REFERENCES Table1(UUID)
);
GO

INSERT INTO database1..Table2 ([Number2], [Text2] ) VALUES
(1, 'One'),
(2, 'Two'),
(3, 'Three');
GO

SELECT * FROM database1..Table2;
GO

IF EXISTS (SELECT * FROM sys.views JOIN sys.schemas ON sys.views.schema_id = sys.schemas.schema_id WHERE 
  -- sys.schemas.name = N'SchemaName' AND 
  sys.views.name = N'View1')
DROP VIEW View1
GO

CREATE VIEW View1
AS
  -- body of the view
  SELECT [UUID],
    [Number1],
    [Text1]
  FROM database1..table1
GO

SELECT * FROM database1..View1
GO