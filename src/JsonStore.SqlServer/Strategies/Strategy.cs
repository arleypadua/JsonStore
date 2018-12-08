using System;
using SqlKata.Compilers;

namespace JsonStore.SqlServer.Strategies
{
    public abstract class Strategy
    {
        protected static readonly SqlServerCompiler Compiler = new SqlServerCompiler();

        public abstract string GetCommand();
    }

    public abstract class Strategy<TDocument, TId, TContent> : Strategy
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