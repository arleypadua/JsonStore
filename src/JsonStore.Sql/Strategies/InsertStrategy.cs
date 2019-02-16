using SqlKata;

namespace JsonStore.Sql.Strategies
{
    internal class InsertStrategy<TDocument, TId, TContent> : Strategy<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        internal override SqlResult GetCommand()
        {
            var query = new Query(CollectionInstance.Name)
                .AsInsert(CollectionInstance.GetIndexedValues(Document));
            
            return Compiler.Compile(query);
        }
        
        internal InsertStrategy(Collection<TDocument, TId, TContent> collection, TDocument document) 
            : base(collection, document)
        {
        }
    }
}