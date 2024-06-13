SELECT TOP (1000) [CustomerID]
      ,[CustomerName]
      ,[CustomerNickName]
      ,[CustomerEmail]
      ,[CustomerCellPhone]
      ,[CustomerLocalPhone]
      ,[CustomerAddressCity]
      ,[CustomerAddressDistrict]
      ,[CustomerAddressDetails]
      ,[CustomerAddressMemo]
      ,[CustomerEinvoiceNumber]
      ,[CustomerPassword]
  FROM [Vita].[dbo].[Customers]

  SELECT * FROM AspNetUsers
  WHERE Email = '7ronggg@gmail.com'

  DELETE AspNetUsers
  WHERE Email = '7ronggg@gmail.com'

  SELECT * FROM Customers
  WHERE CustomerEmail = '7ronggg@gmail.com'

  DELETE Customers  
  WHERE CustomerEmail = '7ronggg@gmail.com'

  
  SELECT * FROM Products
  WHERE ProductID = 7

UPDATE Products
SET ProductImage = ''
WHERE ProductID = 7;