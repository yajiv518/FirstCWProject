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
        private string _connString = ConfigurationManager.ConnectionStrings["DatabaseConncet"].ConnectionString;
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
            param.Add("v_fuelEco", stock.FuelEconomy>0?stock.FuelEconomy:0, direction: ParameterDirection.Input);
   
               
            return param;
        }

        public ReadStock Create(Stocks stock)
        {
            ReadStock stockDetail = new ReadStock();
            int newId;
            try
            {
                var param = FillParam(stock);
                
                param.Add("v_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    stockDetail=conn.Query<ReadStock>("sp_UsedCarsCreate", stock, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                //newId = param.Get<int>("v_id");
            }
            catch (Exception)
            {
                
                throw;
            }
            return stockDetail;
        }

        public ReadStock Edit(int id, Stocks stock)
        {
            ReadStock stockDetail = new ReadStock();
            try
            {

                var param = FillParam(stock);
                stock.StockId = id;
                param.Add("v_id", id, direction: ParameterDirection.Input);

                param.Add("v_tag", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    stockDetail = conn.Query<ReadStock>("sp_UsedCarsEdit", stock, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception)
            {
                
                throw;
            }
            return stockDetail;
        }

        public int Delete(int id)
        {
            int tag=0;
            try
            {
                var param = new DynamicParameters();
            param.Add("v_id", id, direction: ParameterDirection.Input);
                param.Add("v_tag", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                conn.Execute("sp_UsedCarsDelete", param, commandType: CommandType.StoredProcedure);
                }
                tag = param.Get<int>("v_tag");
            }
            catch (Exception)
            {
                
                throw;
            }
            return tag;
        }


        public ReadStock Read(int id)
        {
          
            ReadStock stockDetail=new ReadStock();
            try
            {
                var param = new DynamicParameters();
                param.Add("carid", id, direction: ParameterDirection.Input);
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    stockDetail = conn.Query<ReadStock>("sp_UsedCarsGetData", param, commandType: CommandType.StoredProcedure).AsList()[0];
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return stockDetail;
        }

        public IEnumerable<ESGetDetail> GetAllStockDetail()
        {
            var param = new DynamicParameters();
            IEnumerable<ESGetDetail> stockDetail;
            using (IDbConnection conn = new MySqlConnection(_connString))
            {
                stockDetail = conn.Query<ESGetDetail>("sp_AllUsedCarsGetData", param, commandType: CommandType.StoredProcedure);
            }

            return stockDetail;
        }

    }
}