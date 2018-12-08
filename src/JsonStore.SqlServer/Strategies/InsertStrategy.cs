using SqlKata;

namespace JsonStore.SqlServer.Strategies
{
    public class InsertStrategy<TDocument, TId, TContent> : Strategy<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        public override SqlResult GetCommand()
        {
            var query = new Query(CollectionInstance.Name)
                .AsInsert(CollectionInstance.GetIndexedValues(Document));
            
            return Compiler.Compile(query);
        }


        public InsertStrategy(Collection<TDocument, TId, TContent> collection, TDocument document) 
            : base(collection, document)
        {
        }
    }
}