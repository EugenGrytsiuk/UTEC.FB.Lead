using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UTEC.FB.Lead.FB_Data
{
    public class DB_StoredP
    {
        private static string connectionString = "";


        public static void AddRequest(string description, string name, long phone, string res)
        {

            string sqlExpresion = "sm_add_customer_request";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpresion, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter Description = new SqlParameter
                {
                    ParameterName = "@Description ",
                    Value = description
                };
                command.Parameters.Add(Description);
                SqlParameter Name = new SqlParameter
                {
                    ParameterName = "@NAME",
                    Value = name
                };
                command.Parameters.Add(Name);
                SqlParameter Phone = new SqlParameter
                {
                    ParameterName = "@Phone",
                    Value = phone
                };
                command.Parameters.Add(Phone);
                //SqlParameter Worker_id = new SqlParameter
                //{
                //    ParameterName = "@Worker_id",
                //    Value = workerId
                //};
                //command.Parameters.Add(Worker_id);
                SqlParameter Res = new SqlParameter
                {
                    ParameterName = "@Res",
                    Value = "other"
                };
                command.Parameters.Add(Res);

                try
                {
                    var result = command.ExecuteScalar();
                    Console.WriteLine("added item id is {0}", result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}