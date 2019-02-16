using System;
using Dapper;
using JsonStore.Abstractions;
using JsonStore.Sql.Model;
using JsonStore.Sql.Strategies;
using SqlKata;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JsonStore.Sql
{
    public class SqlServerDocumentStore : IStoreDocuments
    {
        private readonly IDbConnection _dbConnection;

        public SqlServerDocumentStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task StoreAsync<TDocument, TId, TContent>(Collection<TDocument, TId, TContent> collection) 
            where TDocument : Document<TContent, TId>, new()
            where TContent : class
        {
            var commands = collection
                .GetModifiedDocuments()
                .Select(doc => StrategyFactory.GetStrategy(collection, doc));
            
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands)
                    {
                        await command.Execute(_dbConnection, transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<string> GetDocumentContentById<TDocument, TId, TContent>(Collection<TDocument, TId, TContent> collection, TId id) 
            where TDocument : Document<TContent, TId>, new() 
            where TContent : class
        {
            var query = new Query(collection.Name)
                .Where(Collection.IdKey, id);

            var sqlResult = Strategy.Compiler.Compile(query);

            var record = (await _dbConnection.QueryAsync<SqlDocument>(sqlResult.Sql, sqlResult.NamedBindings))
                .FirstOrDefault();
            
            return record?._Document;
        }
    }
}