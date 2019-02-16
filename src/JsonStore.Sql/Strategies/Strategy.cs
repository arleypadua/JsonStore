using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SqlKata;
using SqlKata.Compilers;

namespace JsonStore.Sql.Strategies
{
    internal abstract class Strategy
    {
        protected static readonly SqlServerCompiler Compiler = new SqlServerCompiler();

        internal abstract SqlResult GetCommand();

        internal virtual Task Execute(IDbConnection connection, IDbTransaction transaction = null)
        {
            var command = GetCommand();

            return connection.ExecuteAsync(
                command.Sql,
                command.NamedBindings,
                transaction,
                30);
        }
    }

    internal abstract class Strategy<TDocument, TId, TContent> : Strategy
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        protected Strategy(Collection<TDocument, TId, TContent> collection, TDocument document)
        {
            CollectionInstance = collection ?? throw new ArgumentNullException(nameof(collection));
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        protected TDocument Document { get; }
        protected Collection<TDocument, TId, TContent> CollectionInstance { get; }
    }
}