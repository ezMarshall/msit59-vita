-- �վ� StorePassword ��쪺��������� nvarchar(512)
ALTER TABLE Stores
ALTER COLUMN StorePassword nvarchar(512);

-- ��s StoreID = 1 �� StorePassword �� '����ȱK�X'
UPDATE Stores
SET StorePassword = 'AQAAAAIAAYagAAAAEHQHEVQ6jFflWQn8xy5FbZgjBla5fJ08EBY/BiBrLWh0Aak3vwCOcUSSAmGxJ8axDA=='
WHERE StoreID = 1;