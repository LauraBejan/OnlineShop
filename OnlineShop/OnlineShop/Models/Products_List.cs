using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace OnlineShop.Models
{
    public class Products_List
    {
        public int product_list_id;
        public int total_price;
        public List<Products_Info> products_info_list = new List<Products_Info>();

        public Products_List()
        {
            total_price = 0;

        }

        public int GetMaxListId(Connection connection)
        {
            int max_id = -1;
            try
            {
                if (connection._connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                var command = new Oracle.ManagedDataAccess.Client.OracleCommand("getMaximumListId", connection._connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                Oracle.ManagedDataAccess.Client.OracleParameter output = command.Parameters.Add("index_max", OracleDbType.Int32);
                command.ExecuteNonQuery();
                max_id =Int32.Parse( command.Parameters["index_max"].Value.ToString());
                connection.Close();

            }
            catch (Exception e)
            {
                throw;
             //   connection.Close();

            }
            finally
            {
            }
            return max_id;

        }

        public void InsertProductInList(int list_id, int product_id, int quantity, Connection connection)
        {
            try
            {
                if (connection._connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                var command = new Oracle.ManagedDataAccess.Client.OracleCommand("insert_product_info", connection._connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("list_id", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = list_id;
                command.Parameters.Add("product_id", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = product_id;
                command.Parameters.Add("quantity_2", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = quantity;

                command.ExecuteNonQuery();

                Oracle.ManagedDataAccess.Client.OracleDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                throw;

            }
        }

        public List<Products_Info> GetDbProductsList(Connection connection,int id)
        {
            try
            {
                if (connection._connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                var command = new Oracle.ManagedDataAccess.Client.OracleCommand("returnCartPro", connection._connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                List<Products_Info> products_Info_list = new List<Products_Info>();

                command.Parameters.Add("ID_INPUT", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = id;
                Oracle.ManagedDataAccess.Client.OracleParameter p_rc = command.Parameters.Add("rc", OracleDbType.RefCursor,
                                        DBNull.Value,
                                        System.Data.ParameterDirection.Output);
               // Oracle.ManagedDataAccess.Client.OracleParameter output = command.Parameters.Add("rc", OracleDbType.RefCursor);
                // output.Direction = System.Data.ParameterDirection.ReturnValue;
                command.ExecuteNonQuery();

                Oracle.ManagedDataAccess.Client.OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Products_Info products_Info = new Products_Info(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetString(4),
                        reader.GetString(5));
                    products_Info_list.Add(products_Info);
                }
                connection.Close();
                return products_Info_list;
                //  return null;
            }
            catch (Exception e)
            {
                connection.Close();
                throw;

            }
        }
    }
}

