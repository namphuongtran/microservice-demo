using System.Data;
using Framework.Automap.SQLServer;

namespace Core.SQL.SQLServer
{
    public interface IConnectSQL
    {
        void ExecuteNonQuery(string query, SQLParameters parameter, ExecuteType type);

        DataTable ExecuteToTable(string query, SQLParameters parameter, ExecuteType type);

        DataSet ExecuteToDataset(string query, SQLParameters parameter, ExecuteType type);

        T ExecuteScalar<T>(string query, SQLParameters parameter, ExecuteType type);

        T ExecuteScalarFunction<T>(string query, SQLParameters parameter, ExecuteType type);
    }
}
