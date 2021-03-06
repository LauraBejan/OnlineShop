﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace OnlineShop.Models
{

    public class Available_products
    {
        public int available_product_id;
        public string product_name;
        public string category;
        public int price;
        public int stock;
        public string material;

        public  List<Available_products> GetProducts(Connection connection)
        {
            try
            {
                if(connection._connection.State== System.Data.ConnectionState.Closed)
                    connection.Open();
                var command = new OracleCommand("PRODUCTS", connection._connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                List<Available_products> available_products_list = new List<Available_products>();
                OracleParameter output = command.Parameters.Add("l_cursor", OracleDbType.RefCursor);
                output.Direction = System.Data.ParameterDirection.ReturnValue;
                command.ExecuteNonQuery();

                OracleDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Available_products available_products = new Available_products();
                    available_products.available_product_id = reader.GetInt32(0);
                    available_products.category = reader.GetString(1);
                    available_products.stock = reader.GetInt32(2);
                    available_products.material = reader.GetString(3);
                    available_products.price = reader.GetInt32(4);
                    available_products.product_name = reader.GetString(5);
                    available_products_list.Add(available_products);
                }
                connection.Close();
                return available_products_list;
              //  return null;
            }
           catch(Exception e)
            {
                connection.Close();
                throw;
        
            }
            finally
            {
            }
   
    }
}
    }