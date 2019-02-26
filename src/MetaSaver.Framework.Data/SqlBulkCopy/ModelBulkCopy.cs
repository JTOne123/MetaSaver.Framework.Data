using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MetaSaver.Framework.Configuration;
using Microsoft.Extensions.Options;

// Modified from a post by Dejan Stojanovic
// https://dejanstojanovic.net/aspnet/2016/january/sqlbulkcopy-with-model-classes-in-c/

// ReSharper disable once CheckNamespace
namespace MetaSaver.Framework.Data
{
    #region Interface

    public interface IModelBulkCopy
    {
        void WriteToServer<T>(IEnumerable<T> models, string tableName, IDbConnection connection)
            where T : class, new();
    }

    #endregion

    #region Class

    public class ModelBulkCopy : IModelBulkCopy
    {
        #region Locals

        private readonly int _bulkCopyTimeout;
        private readonly int _batchSize;

        #endregion

        #region Constructor

        public ModelBulkCopy(IOptions<BaseAppSettings> appSettings)
        {
            _bulkCopyTimeout = appSettings.Value.Data.BulkCopy.Timeout;
            _batchSize = appSettings.Value.Data.BulkCopy.BatchSize;
        }

        #endregion

        #region WriteToServer

        public void WriteToServer<T>(IEnumerable<T> models, string tableName, IDbConnection connection)
            where T : class, new()
        {
            using (var bulkCopy = new SqlBulkCopy(connection.ConnectionString))
            {
                var dataTable = ModelMapper.MapModel(models, tableName);

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BulkCopyTimeout = _bulkCopyTimeout;
                bulkCopy.BatchSize = _batchSize;

                bulkCopy.WriteToServer(dataTable);
            }
        }

        #endregion
    }

    #endregion
}
