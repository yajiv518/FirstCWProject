using Entities;
using Interfaces;
using System;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;


namespace DAL
{
    public class DataAccessLayer:IDataAccess<Stocks>//,IDataAccess<ReadStock>
    {
        private string connString = ConfigurationManager.ConnectionStrings["DatabaseConncet"].ConnectionString;
        private DynamicParameters FillParam(Stocks stock)
        {
            var param = new DynamicParameters();
            param.Add("v_price", stock.Price, direction: ParameterDirection.Input);
            param.Add("v_year", stock.Year, direction: ParameterDirection.Input);
            param.Add("v_km", stock.Kilometers, direction: ParameterDirection.Input);
            param.Add("v_fuelID", stock.FuelTypeId, direction: ParameterDirection.Input);
            param.Add("v_cityID", stock.CityId, direction: ParameterDirection.Input);
            param.Add("v_colorID", stock.ColorId, direction: ParameterDirection.Input);
            param.Add("v_makeID", stock.MakeId, direction: ParameterDirection.Input);
            param.Add("v_modelID", stock.ModelId, direction: ParameterDirection.Input);
            param.Add("v_versionID", stock.VersionId, direction: ParameterDirection.Input);
            if (stock.FuelEconomy != 0.0)
                param.Add("v_fuelEco", stock.FuelEconomy, direction: ParameterDirection.Input);
            else
                param.Add("v_fuelEco", 0.0, direction: ParameterDirection.Input);
            return param;
        }

        public int Create(Stocks stock)
        {
            int newId;
            var param = FillParam(stock);
            param.Add("v_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using(IDbConnection conn=new MySqlConnection(connString))
            {
                conn.Execute("sp_UsedCarsCreate",param,commandType:CommandType.StoredProcedure);
            }
            newId = param.Get<int>("v_id");
            return newId;
        }

        public int Edit(int id,Stocks stock)
        {
            int tag=0;
            var param = FillParam(stock);
            param.Add("v_id", id, direction: ParameterDirection.Input);
            param.Add("v_tag", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (IDbConnection conn = new MySqlConnection(connString))
            {
                conn.Execute("sp_UsedCarsEdit", param, commandType: CommandType.StoredProcedure);
            }
            tag = param.Get<int>("v_tag");
            return tag;
        }

        public int Delete(int id)
        {
            int tag=0;
            var param = new DynamicParameters();
            param.Add("v_id", id, direction: ParameterDirection.Input);
            param.Add("v_tag", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (IDbConnection conn = new MySqlConnection(connString))
            {
                conn.Execute("sp_UsedCarsDelete", param, commandType: CommandType.StoredProcedure);
            }
            tag = param.Get<int>("v_tag");
            return tag;
        }


        public ReadStock Read(int id)
        {
            var param = new DynamicParameters();
            ReadStock stockDetail=new ReadStock();
            param.Add("carid", id, direction: ParameterDirection.Input);
            using (IDbConnection conn = new MySqlConnection(connString))
            {
                stockDetail=conn.Query<ReadStock>("sp_UsedCarsGetData", param, commandType: CommandType.StoredProcedure).AsList()[0];
            }

            return stockDetail;
        }

        //public IEnumerable<ReadStock> GetAllStockDetail()
        //{
        //    var param = new DynamicParameters();
        //    IEnumerable<ReadStock> stockDetail;
        //    using (IDbConnection conn = new MySqlConnection(connString))
        //    {
        //        stockDetail = conn.Query<ReadStock>("sp_AllUsedCarsGetData", param, commandType: CommandType.StoredProcedure);
        //    }

        //    return stockDetail;
        //}

    }
}