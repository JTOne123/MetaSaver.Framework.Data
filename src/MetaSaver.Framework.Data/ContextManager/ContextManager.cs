using System;
using System.Data;
using System.Data.SqlClient;
using MetaSaver.Framework.Configuration;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

// ReSharper disable once CheckNamespace
namespace MetaSaver.Framework.Data
{
    #region Interface

    public interface IContextManager : IDisposable
    {
        IDbConnection Connection { get; }
    }

    #endregion

    #region Class

    public class ContextManager : IContextManager
    {
        #region Locals

        private readonly RetryPolicy _policy;
        private IDbConnection _connection;
        private readonly object _syncObject = new object();
        private string ConnectionString { get; }

        #endregion

        #region Constructor

        public ContextManager(IOptions<BaseAppSettings> appSettings)
        {
            ConnectionString = appSettings.Value.Data.Database;

            _policy = Policy.Handle<Exception>().WaitAndRetry(
                10,
                attempt => TimeSpan.FromSeconds(0.1 * Math.Pow(2, attempt)),
                (exception, calculatedWaitDuration) =>
                {
                    GC.Collect(0);
                    GC.Collect(1);
                    GC.Collect(2);
                }
            );
        }

        #endregion

        #region Connection

        public IDbConnection Connection
        {
            get
            {
                lock (_syncObject)
                {
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                            return _connection;

                        _connection = null;
                    }

                    _connection = _policy.Execute(() =>
                    {
                        var connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        return connection;
                    });

                    return _connection;
                }
            }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            _connection?.Dispose();
        }

        #endregion
    }

    #endregion
}