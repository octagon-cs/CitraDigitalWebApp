
using Dapper;
using MySql.Data.MySqlClient;
using WebApp.Models;
using System;
using System.Data.Common;
using System.Data;

namespace WebApp
{
    public class DapperContext
    {

        public static string connectionString=>"server=localhost; database=cdpertamina;uid=root";

        public static MySqlConnection Connection
        {
            get
            {
                return new MySqlConnection(connectionString); ;
            }
        }
    }


}