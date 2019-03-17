using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace System
{
    public  class DbHelperSQL
    {
        protected static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["WebContext"].ToString();
        public static string _QianZhui = "SaiShi_t";
        public DbHelperSQL(string str="")
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[str == "" ? "WebContext" : str].ToString();
        }
       
       private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        
       public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
       {
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               SqlCommand cmd = new SqlCommand();
               PrepareCommand(cmd, connection, null, SQLString, cmdParms);
               using (SqlDataAdapter da = new SqlDataAdapter(cmd))
               {
                   DataSet ds = new DataSet();
                   connection.Close();
                   try
                   {
                       da.Fill(ds, "ds");
                       cmd.Parameters.Clear();
                   }
                   catch (System.Data.SqlClient.SqlException ex)
                   {
                       throw new Exception(ex.Message);
                   }
                   return ds;
               }
           }
       }
        
       public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        connection.Close();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

       public static int ExecTranSql(string Sql, params  SqlParameter[] cmdParms)
        {
            SqlConnection Con = new SqlConnection(connectionString);
            int Ret = 0; SqlTransaction Tran = null;
            try
             {
                 Con.Open();
                 Tran = Con.BeginTransaction();
                 SqlCommand Cmd = new SqlCommand(Sql, Con,Tran);
                 foreach (SqlParameter mm in cmdParms)
                 {
                     Cmd.Parameters.Add(mm);
                 }
                 string Result = Cmd.ExecuteScalar().ToString();
                 Ret =!String.IsNullOrEmpty(Result)? int.Parse(Result):0;
                 Tran.Commit();
                 Con.Close();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                Con.Close();
                throw new Exception(ex.Message);
            }
             return Ret;
        }

       public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                connection.Close();
                return result;
            }
        }
       private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
       private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
    }
}
