using System.Data;
using System.Data.SqlClient;
using Framework.Automap.SQLServer;
using Framework.Entities.Systems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Core.SQL.SQLServer
{
    public class ConnectSQL : IConnectSQL
    {
        #region attribute

        private SqlConnection _con;
        private SqlCommand _cmd;
        private SqlDataAdapter _adapter;

        public static string ConnectionString = string.Empty;
        public static int ConnectDbTimeOut = 300;

        private readonly SQLConfigSingleton _singletonSqlConfig;

        #endregion atribute

        #region Constructor
        public ConnectSQL(IOptions<ConfigDB> options)
        {
            //try
            //{
            //    _singletonSqlConfig = SQLConfigSingleton.Instance;

            //    if (string.IsNullOrEmpty(ConnectionString))
            //    {
            //        ConnectionString = _singletonSqlConfig._connectionString;
            //        ConnectDBTimeOut = _singletonSqlConfig._connectDBTimeOut;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            ConnectionString = options.Value.ConnectionString;
            ConnectDbTimeOut = options.Value.ConnectDBTimeOut;
        }

        public ConnectSQL(IConfigurationRoot configuration)
        {
            ConnectionString = configuration["ConfigDB:ConnectionString"];//StaticConfig.ConnectionString_SQL;
            ConnectDbTimeOut = int.Parse(configuration["ConfigDB:ConnectDBTimeOut"]);//StaticConfig.ConnectDBTimeOut_SQL;
        }
        #endregion Constructor

        #region Methods

        public void ExecuteNonQuery(string query, SQLParameters parameter, ExecuteType type)
        {

            using (_con = new SqlConnection(ConnectionString))
            {
                _con.Open();
                _cmd = new SqlCommand
                {
                    Connection = _con,
                    CommandType = type == ExecuteType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text,
                    CommandText = query,
                    CommandTimeout = ConnectDbTimeOut
                };

                if (parameter != null)
                    _cmd.Parameters.AddRange(parameter.ToArray());

                _cmd.ExecuteNonQuery();
                _con.Dispose();

                if (_con.State == ConnectionState.Open)
                    _con.Close();
            }
        }

        public DataTable ExecuteToTable(string query, SQLParameters parameter, ExecuteType type)
        {
            using (_con = new SqlConnection(ConnectionString))
            {
                _con.Open();
                _cmd = new SqlCommand {Connection = _con};
                _cmd.Parameters.Clear();
                _cmd.CommandType = type == ExecuteType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                _cmd.CommandText = query;
                _cmd.CommandTimeout = ConnectDbTimeOut;

                if (parameter != null)
                    _cmd.Parameters.AddRange(parameter.ToArray());

                _adapter = new SqlDataAdapter(_cmd);
                DataTable tbl = new DataTable();
                _adapter.Fill(tbl);
                _adapter.Dispose();

                if (_con.State == ConnectionState.Open)
                    _con.Close();

                return tbl;
            }
        }

        public T ExecuteScalarFunction<T>(string query, SQLParameters parameter, ExecuteType type)
        {
            using (_con = new SqlConnection(ConnectionString))
            {
                _cmd = new SqlCommand {Connection = _con};
                _cmd.Parameters.Clear();
                _cmd.CommandType = type == ExecuteType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                _cmd.CommandText = query;
                _cmd.CommandTimeout = ConnectDbTimeOut;

                if (parameter != null)
                    _cmd.Parameters.AddRange(parameter.ToArray());
                SqlParameter returnValue = _cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Text);
                returnValue.Direction = ParameterDirection.ReturnValue;

                _con.Open();
                _cmd.ExecuteNonQuery();
                if (_con.State == ConnectionState.Open)
                    _con.Close();

                return (T)returnValue.Value;
            }
        }

        public DataSet ExecuteToDataset(string query, SQLParameters parameter, ExecuteType type)
        {
            using (_con = new SqlConnection(ConnectionString))
            {
                _con.Open();
                _cmd = new SqlCommand {Connection = _con};
                _cmd.Parameters.Clear();
                _cmd.CommandType = type == ExecuteType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                _cmd.CommandText = query;
                _cmd.CommandTimeout = ConnectDbTimeOut;

                if (parameter != null)
                    _cmd.Parameters.AddRange(parameter.ToArray());

                _adapter = new SqlDataAdapter(_cmd);
                DataSet dts = new DataSet();
                _adapter.Fill(dts);
                _adapter.Dispose();

                if (_con.State == ConnectionState.Open)
                    _con.Close();

                return dts;
            }
        }


        public T ExecuteScalar<T>(string query, SQLParameters parameter, ExecuteType type)
        {
            using (_con = new SqlConnection(ConnectionString))
            {
                _con.Open();
                _cmd = new SqlCommand
                {
                    Connection = _con,
                    CommandType = type == ExecuteType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text,
                    CommandText = query,
                    CommandTimeout = ConnectDbTimeOut
                };

                if (parameter != null)
                    _cmd.Parameters.AddRange(parameter.ToArray());

                object obj = _cmd.ExecuteScalar();
                _con.Close();

                return (T)obj;
            }
        }


        #endregion 
    }

    public enum ExecuteType
    {
        SqlQuery = 0,
        StoredProcedure = 1
    }
}