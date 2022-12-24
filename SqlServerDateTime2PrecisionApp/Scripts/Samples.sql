--- get precision for datetime2 column
SELECT 	TABLE_NAME,COLUMN_NAME,DATETIME_PRECISION FROM INFORMATION_SCHEMA.COLUMNS WHERE  DATA_TYPE = 'datetime2';

--- precision 2 with a datetime(7) column
SELECT CAST('2022-11-26 17:44:28.4006356' AS datetime2(2)) AS 'Created' 
