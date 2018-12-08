using SqlKata;

namespace JsonStore.SqlServer.Strategies
{
    public class InsertStrategy<TDocument, TId, TContent> : Strategy<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        public override string GetCommand()
        {
            var query = new Query(CollectionInstance.Name)
                .AsInsert(CollectionInstance.GetIndexedValues(Document));
            
            return Compiler.Compile(query).ToString();
        }


        public InsertStrategy(Collection<TDocument, TId, TContent> collection, TDocument document) 
            : base(collection, document)
        {
        }
    }
}