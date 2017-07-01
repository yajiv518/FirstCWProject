﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Elasticsearch.Net.ConnectionPool;


namespace Carwale.DAL.CoreDAL
{
    public sealed class ElasticClientInstance
    {
        private static readonly ElasticClientInstance _clientInstance = new ElasticClientInstance();

        private ElasticClient _client;

        static ElasticClientInstance()
        {

        }
        public static ElasticClient GetInstance()
        {
            return _clientInstance._client;
        }
        private ElasticClientInstance()
        {
            try
            {
                Uri[] nodes = ConfigurationManager.AppSettings["ElasticHostUrl"].Split(';')
                               .Select(s => new Uri("http://" + s)).ToArray();
                var connectionPool = new SniffingConnectionPool(nodes);
                var settings = new ConnectionSettings(
                    connectionPool,
                    defaultIndex: ConfigurationManager.AppSettings["ElasticIndexName"]
                ).SetTimeout(1000 * 30);

                _client = new ElasticClient(settings);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
