using System;
using Framework.Entities.Systems;
using Microsoft.Extensions.Options;

namespace Core.SQL.SQLServer
{
    public sealed class SQLConfigSingleton
    {
        private const string SqlConfig = "ConnectionString";
        private const string SqlTimeout = "ConnectDBTimeOut";
        public string ConnectionString { set; get; }
        public int ConnectDbTimeOut { set; get; }

        private static volatile SQLConfigSingleton instance;

        private static object syncRoot = new Object();

        public ConfigDB Configuration { set; get; }

        private SQLConfigSingleton()
        {
            ConnectionString = Configuration.ConnectionString;//GetConnectionString(_SQL_CONFIG);

            ConnectDbTimeOut = 300;//int.Parse(Configuration[SQL_TIMEOUT]);
        }

        private SQLConfigSingleton(IOptions<ConfigDB> settings)
        {
            Configuration = settings.Value;
            //ConnectionString = "Data Source=.;Initial Catalog=MS_Order;User ID=sa;Password=sa@@@@@@;MultipleActiveResultSets=True";//Configuration[SQL_CONFIG];
            ConnectionString = Configuration.ConnectionString;//GetConnectionString(_SQL_CONFIG);

            ConnectDbTimeOut = 300;//int.Parse(Configuration[SQL_TIMEOUT]);
        }

        public static SQLConfigSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SQLConfigSingleton();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
