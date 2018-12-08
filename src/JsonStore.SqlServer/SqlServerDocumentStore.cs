using JsonStore.Abstractions;
using JsonStore.SqlServer.Strategies;
using System.Linq;
using System.Threading.Tasks;

namespace JsonStore.SqlServer
{
    public class SqlServerDocumentStore : IStoreDocuments
    {
        public Task StoreAsync<TDocument, TId, TContent>(Collection<TDocument, TId, TContent> collection) 
            where TDocument : Document<TContent, TId>, new()
            where TContent : class
        {
            var commands = collection
                .GetModifiedDocuments()
                .Select(doc => StrategyFactory.GetStrategy(collection, doc))
                .Select(s => s.GetCommand());

            var commandsString = string.Join(";", commands);

            // TODO execute commands

            return  Task.CompletedTask;
        }
    }
}