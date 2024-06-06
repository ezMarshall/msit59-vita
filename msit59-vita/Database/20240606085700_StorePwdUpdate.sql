-- 調整 StorePassword 欄位的資料類型為 nvarchar(512)
ALTER TABLE Stores
ALTER COLUMN StorePassword nvarchar(512);

-- 更新 StoreID = 1 的 StorePassword 為 '雜湊值密碼'
UPDATE Stores
SET StorePassword = 'AQAAAAIAAYagAAAAEHQHEVQ6jFflWQn8xy5FbZgjBla5fJ08EBY/BiBrLWh0Aak3vwCOcUSSAmGxJ8axDA=='
WHERE StoreID = 1;