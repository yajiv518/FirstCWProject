﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Elasticsearch.Net.ConnectionPool;
//using Carwale.Notifications; 

namespace Carwale.DAL.CoreDAL
{
    public sealed class ElasticClientInstance
    {
        private static readonly ElasticClientInstance _clientInstance = new ElasticClientInstance();

        private ElasticClient _client;

        static ElasticClientInstance () {
            
        }

        public static ElasticClient GetInstance() {
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
                ).SetTimeout(1000 * 30) ;    // 30 seconds timeout
                 //.MaximumRetries(3)         // 3 times retry
                 //.SniffOnConnectionFault(true)
                 //.SniffOnStartup(true)
                 //.SniffLifeSpan(TimeSpan.FromMinutes(1));

                _client = new ElasticClient(settings);
            }
            catch (Exception ex) 
            {
              //  var objErr = new ExceptionHandler(ex, "ElasticClientInstance.ElasticClientInstance()" + ex.InnerException);
               // objErr.LogException();
            }

        }
    }
}
