using Carwale.DAL.CoreDAL;
using DAL;
using Entities;
using Interfaces;
using MySql.Data.MySqlClient;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Configuration;

namespace ElasticSearch
{
    public class SyncESwithDatabase
    {
        public IEnumerable<ESGetDetail> GetAllStockDetail()
        {
            string connString = ConfigurationManager.ConnectionStrings["DatabaseConncet"].ConnectionString;
            IEnumerable<ESGetDetail> stockDetail;
            using (IDbConnection conn = new MySqlConnection(connString))
            {
                stockDetail = conn.Query<ESGetDetail>("sp_AllUsedCarsGetData", commandType: CommandType.StoredProcedure);
            }

            return stockDetail;
        }

        public void CreateIndex()
        {
            ElasticClient client = ElasticClientInstance.GetInstance();
            var temp = client.CreateIndex("stockdata_g3_1", c => c
                         .NumberOfReplicas(0)
                         .NumberOfShards(1)
                         .AddMapping<ESGetDetail>(m => m.MapFromAttributes())
                     );
        }
        public void SyncIndex()
        {
           // IDataAccess<Stocks> data = new DataAccessLayer();
            //IDataAccess<ReadStock> d = new DataAccessLayer();
            IEnumerable<ESGetDetail> allStockData = GetAllStockDetail();
            ElasticClient client = ElasticClientInstance.GetInstance();
            //CreateIndex();
            int id = 1;
            foreach (ESGetDetail read in allStockData)
            {
                id = read.ID;
                client.Index(read, i => i
                    .Index("stockdata_g3_1")
                    .Type("esgetdetail")
                    .Id(id.ToString())
                         );
            }
        }
    }
}
