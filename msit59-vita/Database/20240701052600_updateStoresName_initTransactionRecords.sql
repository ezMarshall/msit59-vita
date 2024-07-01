
USE Vita
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(台中一中店)' WHERE StoreID = 54
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(彰化員林店)' WHERE StoreID = 55
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(台中北屯店)' WHERE StoreID = 56
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(台中大里店)' WHERE StoreID = 57
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(台中豐原店)' WHERE StoreID = 58
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(台中沙鹿店)' WHERE StoreID = 59
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(嘉義朴子店)' WHERE StoreID = 60
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(台南新營店)' WHERE StoreID = 61
UPDATE Stores SET StoreName = '隨主食法式水煮專賣(新竹竹科店)' WHERE StoreID = 62

CREATE TABLE TransactionRecords(
TransactionRecordsID INT IDENTITY(1,1) NOT NULL,
OrderID INT NOT NULL CONSTRAINT FK_TransactionRecords_Orders REFERENCES Orders(OrderID),--根據FK orderID
TransactionID VARCHAR(25) NOT NULL,--每筆交易的ID
TransactionTime DATETIME NOT NULL,--交易時間
TransactionTimestamp VARCHAR(15) NOT NULL,--預給的orderID
TransactionType TINYINT NOT NULL, --交易種類 0 授權 1 請款 2 取消授權 3退款 etc...
TransactionResult BIT NOT NULL,--交易成功與否 0否 1是
)

ALTER TABLE TransactionRecords
ADD CONSTRAINT PK_TransactionRecordsID 
PRIMARY KEY (TransactionRecordsID); 