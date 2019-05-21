using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace OnlineShop
{
    class Connection
    {
        public Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDataBase"].ConnectionString;

            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = connectionString;
            _connection.Open();
            string sql = "select * from image";

            OracleCommand command = new OracleCommand(sql, _connection);
            Console.WriteLine(command);
            _connection.Close();
        }
    }
}
