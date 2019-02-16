using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JsonStore.Abstractions;
using JsonStore.Sql.Strategies;

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
                foreach (var command in commands)
                {
                    await command.Execute(_dbConnection, transaction);
                }
            }
        }
    }
}