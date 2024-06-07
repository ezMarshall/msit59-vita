select LEN(CustomerCellPhone) from Customers

----------------
select LEN(CustomerLocalPhone) from Customers -- ID 1~30 電話僅有9碼
select CustomerLocalPhone from Customers 
UPDATE Customers
SET CustomerLocalPhone = '0423015896'
WHERE CustomerID = 1;

UPDATE Customers
SET CustomerLocalPhone = '0424176932'
WHERE CustomerID = 2;

UPDATE Customers
SET CustomerLocalPhone = '0421358746'
WHERE CustomerID = 3;

UPDATE Customers
SET CustomerLocalPhone = '0420983647'
WHERE CustomerID = 4;

UPDATE Customers
SET CustomerLocalPhone = '0424635189'
WHERE CustomerID = 5;

UPDATE Customers
SET CustomerLocalPhone = '0422107653'
WHERE CustomerID = 6;

UPDATE Customers
SET CustomerLocalPhone = '0423659278'
WHERE CustomerID = 7;

UPDATE Customers
SET CustomerLocalPhone = '0421493027'
WHERE CustomerID = 8;

UPDATE Customers
SET CustomerLocalPhone = '0425063819'
WHERE CustomerID = 9;

UPDATE Customers
SET CustomerLocalPhone = '0421937465'
WHERE CustomerID = 10;

UPDATE Customers
SET CustomerLocalPhone = '0423185706'
WHERE CustomerID = 11;

UPDATE Customers
SET CustomerLocalPhone = '0422768054'
WHERE CustomerID = 12;

UPDATE Customers
SET CustomerLocalPhone = '0423816945'
WHERE CustomerID = 13;

UPDATE Customers
SET CustomerLocalPhone = '0424598312'
WHERE CustomerID = 14;

UPDATE Customers
SET CustomerLocalPhone = '0421854673'
WHERE CustomerID = 15;

UPDATE Customers
SET CustomerLocalPhone = '0422738906'
WHERE CustomerID = 16;

UPDATE Customers
SET CustomerLocalPhone = '0424093165'
WHERE CustomerID = 17;

UPDATE Customers
SET CustomerLocalPhone = '0421479826'
WHERE CustomerID = 18;

UPDATE Customers
SET CustomerLocalPhone = '0422056317'
WHERE CustomerID = 19;

UPDATE Customers
SET CustomerLocalPhone = '0424367509'
WHERE CustomerID = 20;

UPDATE Customers
SET CustomerLocalPhone = '0421289647'
WHERE CustomerID = 21;

UPDATE Customers
SET CustomerLocalPhone = '0423705814'
WHERE CustomerID = 22;

UPDATE Customers
SET CustomerLocalPhone = '0421153978'
WHERE CustomerID = 23;

UPDATE Customers
SET CustomerLocalPhone = '0424978603'
WHERE CustomerID = 24;

UPDATE Customers
SET CustomerLocalPhone = '0423427198'
WHERE CustomerID = 25;

UPDATE Customers
SET CustomerLocalPhone = '0422816037'
WHERE CustomerID = 26;

UPDATE Customers
SET CustomerLocalPhone = '0421568740'
WHERE CustomerID = 27;

UPDATE Customers
SET CustomerLocalPhone = '0424135867'
WHERE CustomerID = 28;

UPDATE Customers
SET CustomerLocalPhone = '0422397506'
WHERE CustomerID = 29;

UPDATE Customers
SET CustomerLocalPhone = '0424809315'
WHERE CustomerID = 30;

----------------
select LEN(CustomerEinvoiceNumber) from Customers --ID = 3有九碼

update Customers
set CustomerEinvoiceNumber = '/Q8R9ST7'
where CustomerID = 5

update Customers
set CustomerEinvoiceNumber = '/I7J8KL0'
where CustomerID = 3

----------------
select LEN(StorePhoneNumber) from Stores
select LEN(OrderPhoneNumber) from Orders
----------------
select LEN(OrderEinvoiceNumber) from Orders

select distinct CustomerID from Orders
where LEN(OrderEinvoiceNumber)>8

update Orders
set OrderEinvoiceNumber = '/Q8R9ST7'
where CustomerID = 5

update Orders
set OrderEinvoiceNumber = '/I7J8KL0'
where CustomerID = 3
----------------
--更新訂餐外送的地址

UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000001';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000003';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000004';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000005';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000006';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000009';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000010';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000012';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000016';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000018';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000019';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000022';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000023';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000024';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000025';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000026';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000028';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000032';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000033';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000036';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000037';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000038';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '美村路一段262號' WHERE OrderID = '10000039';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000040';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000041';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000043';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000046';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000048';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000050';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000051';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000053';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000054';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000056';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000058';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000059';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000060';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000061';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000063';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000067';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000068';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000071';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000072';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000074';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000076';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000077';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000080';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000082';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000084';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000085';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000087';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000088';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000090';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩七街55號' WHERE OrderID = '10000092';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000094';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000095';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000098';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000100';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000101';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000102';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000103';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000104';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000106';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000108';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000111';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000113';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000115';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000118';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000119';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000122';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩七街55號' WHERE OrderID = '10000123';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000125';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000126';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000128';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000129';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000130';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000132';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000133';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000135';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000138';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000139';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000140';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000142';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000144';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000147';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '美村路一段262號' WHERE OrderID = '10000148';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000151';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000152';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000153';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000154';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000155';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000156';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '忠明路789號' WHERE OrderID = '10000157';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000159';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000160';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000163';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000164';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000165';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000168';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000169';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩七街55號' WHERE OrderID = '10000171';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000172';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000173';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000175';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000176';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000177';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000179';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000180';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000183';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000185';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000187';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000188';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000190';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000192';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000195';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000196';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000197';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000198';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000200';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000201';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '忠明路789號' WHERE OrderID = '10000202';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000203';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000204';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000205';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000207';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000208';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000209';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000210';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000211';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000214';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000218';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000219';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000220';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000222';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000224';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '忠明路789號' WHERE OrderID = '10000226';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000227';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000228';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000230';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000231';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000232';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩七街55號' WHERE OrderID = '10000234';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000236';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000239';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000240';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000242';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000243';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000246';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000247';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000248';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000250';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000253';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000254';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000255';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000258';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000263';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000265';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000267';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000271';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000276';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000277';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000278';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000279';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000281';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000283';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000284';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000285';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '忠明路789號' WHERE OrderID = '10000288';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000289';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000291';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '中清路一段24號' WHERE OrderID = '10000294';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000295';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000296';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000297';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000298';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000302';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000303';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000305';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000307';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000308';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000312';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '忠明路789號' WHERE OrderID = '10000313';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000314';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000317';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000318';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000319';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000321';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000323';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000324';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '美村路一段262號' WHERE OrderID = '10000325';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000326';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000327';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000328';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000329';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000330';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000335';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000336';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000339';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '忠明路789號' WHERE OrderID = '10000340';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000345';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000346';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000350';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000351';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000353';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000354';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000355';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000356';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000359'; 
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000360';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000364';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000365';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000371';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000372';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000373';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000374';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段57號' WHERE OrderID = '10000375';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000379';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段890號' WHERE OrderID = '10000380';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000381';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000383';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000384';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '向上路567號' WHERE OrderID = '10000388';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000389';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000391';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000392';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '五權西路二段166號' WHERE OrderID = '10000395';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000396';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000397';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '文心路456號' WHERE OrderID = '10000399';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000400';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段123號' WHERE OrderID = '10000404';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '文心南二路4號' WHERE OrderID = '10000406';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '北區', OrderAddressDetails = '英才路321號' WHERE OrderID = '10000408';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '美村路一段262號' WHERE OrderID = '10000409';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '南屯區', OrderAddressDetails = '大墩路789號' WHERE OrderID = '10000412';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '自由路二段123號' WHERE OrderID = '10000413'; 
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西屯區', OrderAddressDetails = '西屯路二段567號' WHERE OrderID = '10000414';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '西區', OrderAddressDetails = '民生路368巷6號' WHERE OrderID = '10000415';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '中華路一段89號' WHERE OrderID = '10000416';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '東區', OrderAddressDetails = '自由路三段217號3樓' WHERE OrderID = '10000417';
UPDATE Orders SET OrderAddressCity = '台中市', OrderAddressDistrict = '中區', OrderAddressDetails = '綠川西街139號' WHERE OrderID = '10000420';

-------------------------------------------
--待補: 新增优輕食06.02當天客人點非主餐的項目
select os.OrderID, o.OrderTime, c.CategoryName, p.ProductName, os.UnitPrice, os.Quantity from OrderDetails as os
left join Products as p on os.ProductID = p.ProductID
left join ProductCategories as c on c.CategoryID = p.CategoryID
left join Orders as o on o.OrderID = os.OrderID
where cast(o.OrderTime as date) = '2024-06-02'

select distinct os.OrderID, o.OrderTime from OrderDetails as os
left join Products as p on os.ProductID = p.ProductID
left join ProductCategories as c on c.CategoryID = p.CategoryID
left join Orders as o on o.OrderID = os.OrderID
where cast(o.OrderTime as date) = '2024-06-02' and p.StoreID = 1
order by o.OrderTime desc

select p.ProductID, c.CategoryID, c.CategoryName, p.ProductName, p.ProductUnitPrice from Products as p
left join ProductCategories as c on p.CategoryID = c.CategoryID
where p.StoreID = 1

-----
insert into OrderDetails(OrderID,ProductID,UnitPrice,Quantity) values
('10000419',29, 10, 2),
('10000417',25, 85, 1),
('10000413',23, 65, 1),
('10000413',18, 55, 1),
('10000412',27, 100, 1),
('10000412',28, 10, 2),
('10000408',31, 20, 1),
('10000403',24, 65, 1),
('10000403',21, 55, 1)


select * from OrderDetails as os
left join Products as p on os.ProductID = p.ProductID
left join Orders as o on o.OrderID = os.OrderID
where p.StoreID = 1 and p.ProductID > 15 and cast(o.OrderTime as date) = '2024-06-02'

--------------------------------