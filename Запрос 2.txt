SELECT products.Name as 'Product_Name', sellers.Surname, sellers.Name, (sales.Quantity*100 / arrivals.Quantity) as '%' FROM sales 
JOIN sellers on sales.IDSales = sellers.ID
JOIN products on products.ID = sales.IDProd
JOIN arrivals on arrivals.IDProd = products.ID
WHERE (sales.Date > '2013-10-01' and sales.Date < '2013-10-08') and (arrivals.Date > '2013-09-07' and arrivals.Date < '2013-10-08')
ORDER BY products.Name, sellers.Surname, sellers.Name;