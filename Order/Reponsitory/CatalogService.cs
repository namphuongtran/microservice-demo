using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;

namespace Order.Reponsitory
{
    public class CatalogService: ICatalogService
    {
        private readonly IConnectSQL _db;

        public CatalogService(IConnectSQL db)
        {
            _db = db;
        }

        public int GetCountOrder()
        {
            SQLParameters p = new SQLParameters();
            int result = _db.ExecuteScalarFunction<int>("ufn_Test", p, ExecuteType.StoredProcedure);

            return result;
        }
    }
}
