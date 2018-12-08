using SqlKata;

namespace JsonStore.SqlServer.Strategies
{
    public class UpdateStrategy<TDocument, TId, TContent> : Strategy<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        public override string GetCommand()
        {
            var query = new Query(CollectionInstance.Name)
                .AsUpdate(CollectionInstance.GetIndexedValues(Document, ignoreIdKey: true))
                .Where(Collection.IdKey, Document.Id);
            
            return Compiler.Compile(query).RawSql;
        }


        public UpdateStrategy(Collection<TDocument, TId, TContent> collection, TDocument document) 
            : base(collection, document)
        {
        }
    }
}