
CREATE DATABASE Vita
GO

USE Vita
GO

CREATE TABLE Customers(
	CustomerID int identity(1,1) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED,
	CustomerName nvarchar(20) NOT NULL,
	CustomerNickName nvarchar(10),
	CustomerEmail varchar(100) NOT NULL ,
	CustomerCellPhone char(15),
	CustomerLocalPhone char(15),
	CustomerAddressCity nvarchar(10) NOT NULL,
	CustomerAddressDistrict nvarchar(10) NOT NULL,
	CustomerAddressDetails nvarchar(50) NOT NULL,
	CustomerAddressMemo nvarchar(100), --可不填，不超過100字
	CustomerEinvoiceNumber char(15),
	CustomerPassword varchar(50) NOT NULL
)
GO

CREATE TABLE Stores(
	StoreID int identity(1,1) NOT NULL CONSTRAINT PK_Stores PRIMARY KEY CLUSTERED,
	StoreName nvarchar(40) NOT NULL,
	StoreAddressCity nvarchar(10) NOT NULL,
	StoreAddressDistrict nvarchar(10) NOT NULL,
	StoreAddressDetails nvarchar(50) NOT NULL,
	StoreAddressMemo nvarchar(500),
	StorePhoneNumber char(15) NOT NULL,
	StoreImage  varchar(50) NOT NULL,
	StoreUniformInvoiceVia bit NOT NULL,
	StoreAccountNumber varchar(50) NOT NULL,
	StorePassword varchar(20) NOT NULL,
	StoreSetTime smalldatetime NOT NULL, --平台開始服務商家時間
	StoreOrderStatus tinyint NOT NULL, --商家當下可不可以用平台接單
	StoreOnService tinyint NOT NULL, --商家還有使用本平台服務
	StoreEndServiceTime smalldatetime, --商家停用本平台服務的時間
	StoreLinePay bit NOT NULL -- 商家接不接受LinePay



)
GO

CREATE TABLE Favorites(
	FavoriteID int identity(1,1) NOT NULL CONSTRAINT PK_Favorites PRIMARY KEY,
	CustomerID int NOT NULL CONSTRAINT FK_Favorites_Customers REFERENCES Customers(CustomerID),
	StoreID int NOT NULL CONSTRAINT FK_Favorites_Stores REFERENCES Stores(StoreID),
	



)



CREATE TABLE Orders (
  OrderID int identity(10000000,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED,
  CustomerID int NOT NULL CONSTRAINT FK_Orders_Customers REFERENCES Customers(CustomerID),
  StoreID int NOT NULL CONSTRAINT FK_Orders_Stores REFERENCES Stores(StoreID),
  OrderTime smalldatetime NOT NULL,
  PredictedArrivalTime smalldatetime NOT NULL,
  OrderDeliveryVia bit NOT NULL,
  OrderPhoneNumber char (15) NOT NULL,
  OrderStoreMemo nvarchar(500), --可不填，不超過500字
  OrderPayment bit NOT NULL,
  OrderUniformInvoiceVia tinyint DEFAULT (0),  --不填的話預設為店家沒開立統一發票
  OrderEinvoiceNumber char(15), --預設為null
  OrderAddressCity nvarchar(10) NOT NULL,
  OrderAddressDistrict nvarchar(10) NOT NULL,
  OrderAddressDetails nvarchar(50) NOT NULL,
  OrderAddressMemo nvarchar(100), --可不填，不超過100字
  CustomerOrderStatus tinyint NOT NULL,
  OrderFinishedTime smalldatetime


  
)
GO

CREATE TABLE OrderMessages(
MessageID int identity(1,1) NOT NULL CONSTRAINT PK_OrderMessages PRIMARY KEY,
OrderID int NOT NULL CONSTRAINT FK_OrderMessages_Orders REFERENCES Orders(OrderID),
MessageInformedTime smalldatetime NOT NULL,
MessageContent tinyint NOT NULL,
MessageStatus bit DEFAULT (0)


)


CREATE TABLE ProductCategories(
	CategoryID int identity(1,1) NOT NULL CONSTRAINT PK_ProductCategories PRIMARY KEY CLUSTERED,
	StoreID int NOT NULL CONSTRAINT FK_ProductCategories_Stores REFERENCES Stores(StoreID),
	CategoryName nvarchar(20) NOT NULL,
	CategoryOnDelete bit NOT NULL

)
GO

CREATE TABLE Products(
	ProductID int identity(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
	StoreID int NOT NULL CONSTRAINT FK_Products_Stores REFERENCES Stores(StoreID),
	ProductName nvarchar(20) NOT NULL,
	CategoryID int NOT NULL CONSTRAINT FK_Products_Categories REFERENCES ProductCategories(CategoryID),
	ProductUnitPrice smallmoney NOT NULL,
	ProductUnitsInStock smallint NOT NULL,
	ProductOnSell bit NOT NULL,
	ProductOnDelete bit NOT NULL,
	ProductImage varchar(50)



)
GO

--INSERT INTO Images (ImageID, ImageName, ImageData)
--VALUES (1, 'example.jpg', BulkColumn FROM OPENROWSET(BULK 'C:\example.jpg', SINGLE_BLOB) AS ImageData);

CREATE TABLE ShoppingCarts(
	ShoppingCartID int identity(1,1) NOT NULL CONSTRAINT PK_ShoppingCarts PRIMARY KEY,
	CustomerID int NOT NULL CONSTRAINT FK_ShoppingCarts_Customers REFERENCES Customers(CustomerID),
	ProductID int NOT NULL CONSTRAINT FK_ShoppingCarts_Products REFERENCES Products(ProductID),
	ShoppingCartQuantity smallint NOT NULL



)


CREATE TABLE OrderDetails (
	OrderID int NOT NULL CONSTRAINT FK_Order_Details_Orders REFERENCES Orders(OrderID),
	ProductID int NOT NULL CONSTRAINT FK_Order_Details_Products REFERENCES Products(ProductID),
	UnitPrice smallmoney NOT NULL,
	Quantity smallint NOT NULL,

	CONSTRAINT PK_Order_Details PRIMARY KEY CLUSTERED 
	(
		OrderID,
		ProductID
	)

)
GO




CREATE TABLE StoreOpeningHours(
	StoreID int NOT NULL CONSTRAINT FK_StoreOpeningHours_Stores REFERENCES Stores(StoreID),
	MyWeekDay nvarchar(10) NOT NULL,
	StoreOpenOrNot bit NOT NULL,
	StoreOpeningTime time NOT NULL,
	StoreClosingTime time NOT NULL
	CONSTRAINT PK_StoreOpeningHours PRIMARY KEY
	(
		StoreID,
		MyWeekDay
	)

)
GO

CREATE TABLE Reviews(
	ReviewID int identity(1,1) NOT NULL CONSTRAINT PK_Reviews PRIMARY KEY,
	OrderID int NOT NULL  CONSTRAINT FK_Reviews_Orders REFERENCES Orders(OrderID),
	ReviewTime smalldatetime NOT NULL,
	ReviewRating tinyint NOT NULL,
	ReviewContent nvarchar(1000),
	StoreReplyTime smalldatetime,
	StoreReplyContent nvarchar(1000)



)
GO








