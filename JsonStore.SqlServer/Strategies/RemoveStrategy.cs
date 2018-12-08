using SqlKata;

namespace JsonStore.SqlServer.Strategies
{
    public class RemoveStrategy<TDocument, TId, TContent> : Strategy<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        public override string GetCommand()
        {
            var query = new Query(CollectionInstance.Name)
                .AsDelete()
                .Where(Collection.IdKey, Document.Id);
            
            return Compiler.Compile(query).RawSql;
        }


        public RemoveStrategy(Collection<TDocument, TId, TContent> collection, TDocument document) 
            : base(collection, document)
        {
        }
    }
}