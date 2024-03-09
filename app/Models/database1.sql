-- Create a new database called 'DatabaseName'
-- Connect to the 'master' database to run this snippet
USE master
GO
-- Create the new database if it does not exist already
IF NOT EXISTS (
  SELECT name FROM sys.databases WHERE name = N'database1'
)
CREATE DATABASE database1
GO


USE database1
-- Create a new table called 'Table1' in schema 'Schema1'
-- Drop the table if it already exists
IF OBJECT_ID('database1..Table1', 'U') IS NOT NULL
DROP TABLE database1..Table1
GO
-- Create the table in the specified schema
CREATE TABLE database1..Table1
(
  UUID INT IDENTITY NOT NULL PRIMARY KEY,
  -- primary key column
  Number1 [INT] NOT NULL,
  Text1 [NVARCHAR](50) NOT NULL
  -- specify more columns here
);
GO

-- Insert rows into table 'TableName'
INSERT INTO database1..Table1
( -- columns to insert data into
 [Number1], [Text1]
)
VALUES
( -- first row: values for the columns in the list above
 1, 'One'
),
( -- second row: values for the columns in the list above
 2, 'Two'
)
-- add more rows here
GO

-- Update rows in table 'TableName'
UPDATE Table1
SET
  [Number1] = 3,
  [Text1] = 'Three'
  -- add more columns and values here
WHERE UUID = 2	/* add search conditions here */
GO

-- Select rows from a Table or View DATABASE1.'TABLE1 schema 'SchemaName'
SELECT * FROM database1..Table1
-- WHERE 	/* add search conditions here */
GO

-- Create a new view called 'ViewName' in schema 'SchemaName'
-- Drop the view if it already exists
IF EXISTS (
SELECT *
  FROM sys.views
  JOIN sys.schemas
  ON sys.views.schema_id = sys.schemas.schema_id
  -- WHERE sys.schemas.name = N'SchemaName'
  AND sys.views.name = N'View1'
)
DROP VIEW View1
GO
-- Create the view in the specified schema
CREATE VIEW View1
AS
  -- body of the view
  SELECT [UUID],
    [Number1],
    [Text1]
  FROM database1..table1
GO

-- Select rows from a Table or View DATABASE1.'TABLE1 schema 'SchemaName'
SELECT * FROM database1..View1
-- WHERE 	/* add search conditions here */
GO




delete from database1..Table1;
go

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
(10, 'Ten')
go