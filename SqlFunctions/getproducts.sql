create or replace 
function GETPRODUCTS 
  return sys_refcursor
is
  l_cursor sys_refcursor;
BEGIN
 open l_cursor
 for
 select AVAILABLE_PRODUCT_ID,CATEGORY,STOCK,MATERIAL,PRICE from available_products;
 return l_cursor;
END ;

