create database BookStoreDB;
use BookStoreDB;

create table Authors (AuthorID int primary key, Name varchar(20), Country varchar(10))
create table Books (BookID int primary key, Title varchar(20), AuthorID int FOREIGN KEY REFERENCES Authors(AuthorID), Price decimal, PublishedYear Date) 
create table Customers (CustomerID int primary key, Name varchar(20) , Email varchar(20), PhoneNumber int) 
create table Orders (OrderID int primary key, CustomerID int FOREIGN KEY REFERENCES Customers(CustomerID), OrderDate Date, TotalAmount decimal) 
create table OrderItems (OrderItemID int primary key, OrderID int, BookID int FOREIGN KEY REFERENCES Orders(OrderID), Quantity int, SubTotal decimal ) 

ALTER TABLE OrderItems
ADD CONSTRAINT FK_OrderItems_Orders
FOREIGN KEY (OrderID) REFERENCES Orders(OrderID);

ALTER TABLE Customers ADD CONSTRAINT UQ_Email UNIQUE (Email);
ALTER TABLE Customers ADD CONSTRAINT UQ_PhoneNumber UNIQUE (PhoneNumber);
INSERT INTO Authors (AuthorID, Name, Country) VALUES
(1, 'Amish Tripathi', 'India'),
(2, 'Arundhati Roy', 'India'),
(3, 'J.K. Rowling', 'UK'),
(4, 'Mark Twain', 'USA'),
(5, 'Gabriel Marquez', 'Colombia');

select * from Authors;


INSERT INTO Books (BookID, Title, AuthorID, Price, PublishedYear) VALUES
(6, 'SQL Mastery', 1, 50, 2024)
(1, 'Malgudi Days', 1, 299.99, 1943),
(2, 'The Blue Umbrella', 2, 199.50, 1974),
(3, 'Five Point Someone', 3, 350.00, 2004),
(4, 'The Immortals of Meluha', 4, 399.99, 2010),
(5, 'The God of Small Things', 5, 450.75, 1997);

ALTER TABLE Books DROP COLUMN PublishedYear;
ALTER TABLE Books ADD PublishedYear INT;
ALTER TABLE Books ALTER COLUMN Title varchar(70)
ALTER TABLE Customers ALTER COLUMN PhoneNumber Bigint


INSERT INTO Customers (CustomerID, Name, Email, PhoneNumber) VALUES
(8, 'navi', 'navi1@email.com', '589089282'),
(2, 'Bob', 'bob@email.com', '9876543210'),
(3, 'Charlie', 'charlie@email.com', '1122334455'),
(4, 'David', 'david@email.com', '5566778899'),
(5, 'Emma', 'emma@email.com', '6677889900');
INSERT INTO Orders (OrderID, CustomerID, OrderDate, TotalAmount) VALUES
(7, 6, '2024-01-15', 900),
(1, 1, '2024-01-15', 599.99),
(2, 2, '2024-02-10', 849.50),
(3, 3, '2024-02-20', 1299.00),
(4, 4, '2024-03-01', 499.99),
(5, 5, '2024-03-05', 999.75);
INSERT INTO OrderItems (OrderItemID, OrderID, BookID, Quantity, SubTotal) VALUES
(1, 1, 1, 2, 599.98),
(2, 2, 2, 1, 199.50),
(3, 3, 3, 3, 1050.00),
(4, 4, 4, 1, 399.99),
(5, 5, 5, 2, 901.50);

--Update the price of a book titled "SQL Mastery" by increasing it by 10%. 
update Books 
Set  Price=price+(price*0.1)
where Title='SQL Mastery';
select * from books where Title='SQL Mastery';

--Delete a customer who has not placed any orders. 
delete Customers
from Customers 
left join Orders on Customers.CustomerID=Orders.CustomerID
where Orders.CustomerID is null




--Oprators 
--1. Retrieve all books with a price greater than 2000. 
select * from Books where price>200

--2. Find the total number of books available. 
SELECT COUNT(*) AS TotalBooks FROM Books;

--3.Find books published between 2015 and 2023.
select * from Books where PublishedYear between 2004 and 2024

--4. Find all customers who have placed at least one order. 
SELECT DISTINCT Customers.CustomerID
FROM Customers
INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID;

SELECT CustomerID 
FROM Orders 
GROUP BY CustomerID;

--5. Retrieve books where the title contains the word "SQL". 
select * from Books where Title Like '%SQL%'

--6.Find the most expensive book in the store. 
select Max(Price) from Books

--7 Retrieve customers whose name starts with "A" or "D". 

select Name from Customers where Name like 'A%' or Name like 'D%'

--8. Calculate the total revenue from all orders. 
select sum(TotalAmount)  as TotalRevenue from Orders 


--Joins
--1. Retrieve all book titles along with their respective author names. 
select a.name as [author names] ,b.Title from Authors a
join books b on b.AuthorID=a.AuthorID

--2. List all customers and their orders (if any). 
select * from Customers C
left join Orders O on O.CustomerID=C.CustomerID

--3. Find all books that have never been ordered.
select b.Title,  O.OrderItemID from Books b
left join OrderItems O on o.BookID!=b.BookID
WHERE o.BookID IS NULL;

--4. Retrieve the total number of orders placed by each customer. 
SELECT c.CustomerID, c.Name, COUNT(o.OrderID) AS TotalOrders
FROM Customers c
LEFT JOIN Orders o ON c.CustomerID = o.CustomerID
GROUP BY c.CustomerID, c.Name;
--5. Find the books ordered along with the quantity for each order.
SELECT o.OrderID, b.Title AS BookTitle, o.Quantity
FROM OrderItems o
JOIN Books b ON o.BookID = b.BookID
ORDER BY o.OrderID
--6. Display all customers, even those who haven’t placed any orders. 
select * from Customers c
left join  Orders O on O.CustomerID!=c.CustomerID
WHERE o.CustomerID IS NULL;
--7. Find authors who have not written any book
SELECT *
FROM Authors a
LEFT JOIN Books b ON a.AuthorID = b.AuthorID
WHERE b.AuthorID IS NULL;


--SubQueries 
--1. Find the customer(s) who placed the first order (earliest OrderDate).
select c.name ,  (select min(OrderDate) from Orders as firstorder where OrderDate is not null) as FisrtDate
from Customers c

select * from Orders

--2. Find the customer(s) who placed the most orders. 
SELECT Name, CustomerID 
FROM Customers 
WHERE CustomerID IN (
    SELECT CustomerID 
    FROM Orders 
    GROUP BY CustomerID 
    HAVING COUNT(OrderID) = (
        SELECT MAX(OrderCount) 
        FROM (SELECT CustomerID, COUNT(OrderID) AS OrderCount FROM Orders GROUP BY CustomerID) AS OrderCounts
    )
);


--3. Find customers who have not placed any orders 
SELECT Name, CustomerID 
FROM Customers 
WHERE CustomerID NOT IN (SELECT DISTINCT CustomerID FROM Orders);


--4. Retrieve all books cheaper than the most expensive book written by( any  author based on your data) 
SELECT Title, Price 
FROM Books 
WHERE Price < (
    SELECT MAX(Price) 
    FROM Books
);

--5. List all customers whose total spending is greater than the average spending of all customers 
