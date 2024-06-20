
--update 10000040 orders, orderdetails, reviews
update orders
set OrderTime='2024-05-29 17:21:00', PredictedArrivalTime= '2024-05-29 17:55:00',OrderFinishedTime='2024-05-29 18:07:00'
where OrderID=10000040

update OrderDetails
set ProductID=12, UnitPrice=100
where OrderID=10000040

update Reviews
set ReviewTime= '2024-05-29 20:25:00', ReviewContent='懷舊滷排骨的調味太重了不好吃'
where OrderID=10000040

--update 10000044 orders, orderdetails, reviews
update orders
set OrderTime='2024-05-28 18:01:00', PredictedArrivalTime= '2024-05-28 18:31:00',OrderFinishedTime='2024-05-28 18:34:00'
where OrderID=10000044

update OrderDetails
set ProductID=12, UnitPrice=100
where OrderID=10000044

update Reviews
set ReviewTime= '2024-05-29 12:15:00', ReviewContent='排骨的部分做得有點過火,口感有些老硬難以嚥下。'
where OrderID=10000044

