

drop database if exists VTCAcaffe;
create database if not exists VTCAcaffe;

use VTCAcaffe;

create table if not exists Customers(
CusID int auto_increment primary key,
CusName nvarchar(50) not null,
userName varchar(50) not null unique,
userPassword varchar(50) not null,
Address nvarchar(255) not null,
PhoneNumber int not null,
Money decimal(10,2) 
);

insert into Customers(CusName, userName,userPassword,Address,PhoneNumber,Money) values
('Doan Ngoc Thach','Thach','1','84 Kim Nguu, Ha Ba Trung, Ha Noi','0392127339',1000.00),
('Pham Duc Toan','Toan123','1','Tan Tien, Hai Duong','01213309289',1000.00);



create table if not exists Orders(

OrderID int auto_increment primary key ,
OrderDate datetime not null,
Note nvarchar(255) ,
OrderStatus nvarchar(20) not null,
CusID int,
constraint fk_Customers_Orders foreign key(CusID) references Customers(CusID)
);


create table if not exists Items(
ItemID int auto_increment primary key,
ItemName nvarchar(255),
ItemPrice decimal(10,2) ,
ItemDescription text ,
Size varchar(10)
);

insert into Items(ItemName, ItemPrice,ItemDescription,Size) values
('Black caffe','15.00',' Black caffeine with a strong flavor, selected from a pure, high quality caffeine that brings you experience different. ','M,X,XL'),
 ('Brown caffe','15.00','Fine coffee with a sweet taste, milk and bitterness of caffeine will give you an unforgettable experience.','M,X,XL'),
 ('Mink incense cafe','25.00',' is the kind of caffeine that you may have heard in many places, with the process of making it quite picky, still retaining the inherent caffeine flavor, the concentration of fleshy concentration, Is it amazing?','M,X,XL'),
 ('Wine cafe','20.00','The unique combination of wine and coffee, is it interesting?','M,X,XL'),
 ('Black coffee roasted nuts','20.00','Rustic but simple, maybe this is what you are looking for.','M,X,XL'),
 ('Lipton ','10.00','Perhaps not everyone can drink coffee, a gentle gentleman with a cup of lipton, why not ?','M,X,XL'),
 ('Mixed fruit','25.00','Including apples, oranges, plums and mangoes to help you have something dessert after trying 1 cup of caffeine and thinking too much. A little sour and sweet bar will make you more comfortable. ','M,L,XL'),
 ('Oolong tea','15.00','A little light, cool bar from nature will help you less heavy in the roll? ','M,X,XL'),
 ('Chrysanthemum tea','15.00','Gentle tea, not sweet, not bitter, and sitting down and doing a little will make your mood better.','M,X,XL'),
 ('Culi caffe','25.00','Culi cafe on the left has only one grain. It has a bitter taste, passionate fragrance, high caffeine content, comparable black water, is it interesting ?','M,X,XL'),
 ('Moka caffe ','30.00','Moka coffee is one of the famous coffee lines belonging to Arabica genus. In Vietnam, moke is a rare coffee, always priced higher than other types. Moka seeds are bigger and more beautiful than other varieties. Its aroma is very special, very luxurious, ecstatic, slightly acidic in an elegant way, for discerning people.','M,X,XL'),
 ('Robusta caffe  ','20.00','Robusta coffee is also known as Robusta coffee. This type of coffee is very suitable for the climate and soil in the Central Highlands of Vietnam, reaching 90-95% of total coffee production annually. This coffee has a strong, non-sour, high caffeine flavor, suitable for Vietnamese tastes.','M,X,XL');

create table if not exists OrderDetail(
OrderID int,
ItemID int ,
ItemCount int,
Size varchar(10),
constraint pk_OrderDetail primary key(OrderID,ItemID,Size),
constraint fk_OrderDetail_Orders foreign key(OrderID) references Orders(OrderID),
constraint fk_OrderDetail_Items foreign key(ItemID) references Items(ItemID)
);
select * from orders;
select * from OrderDetail;
Select Orders.OrderID ,Customers.CusID,Customers.CusName,Customers.Address,Orders.OrderDate,Items.ItemID,Items.ItemName,Items.ItemPrice,OrderDetail.ItemCount,Orders.OrderStatus from Orders inner join Customers on Orders.CusID = Customers.CusID inner join OrderDetail on Orders.OrderID = OrderDetail.OrderID inner join Items on Orderdetail.ItemID = Items.ItemID where Orders.OrderId = 2;
create table if not exists OrderDetail(
OrderID int,
ItemID int ,
ItemCount int,
constraint pk_OrderDetail primary key(OrderID,ItemID),
constraint fk_OrderDetail_Orders foreign key(OrderID) references Orders(OrderID),
constraint fk_OrderDetail_Items foreign key(ItemID) references Items(ItemID)
);
select * from customers;
select * from orderdetail;