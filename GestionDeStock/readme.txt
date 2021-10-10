exercices:
***
https://www.exelib.net/csharp-poo/gestion-d-un-stock.html

EF:
***
PM> add-migration InitialCreate
PM> update-database –verbose
PM> Remove-Migration

SQL:
***
select * from Products

INSERT INTO Products
(id, name, price, Stock)
VALUES
('sneakers-00001', 'nike air', 120, 20);

UPDATE Products
SET name = 'Adidas 4DFWD2'
WHERE id = 'sneakers-00004'
