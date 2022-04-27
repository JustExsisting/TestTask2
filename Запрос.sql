SELECT Surname, Name, Quantity FROM sales JOIN sellers on sales.IDSales = sellers.ID
WHERE (Date >= '2013-10-01' and Date < '2013-10-08')
ORDER BY sellers.Surname, sellers.Name;