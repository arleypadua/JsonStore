using SqlKata;

namespace JsonStore.Sql.Strategies
{
    internal class RemoveStrategy<TDocument, TId, TContent> : Strategy<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        internal override SqlResult GetCommand()
        {
            var query = new Query(CollectionInstance.Name)
                .AsDelete()
                .Where(Collection.IdKey, Document.Id);

            return Compiler.Compile(query);
        }


        internal RemoveStrategy(Collection<TDocument, TId, TContent> collection, TDocument document) 
            : base(collection, document)
        {
        }
    }
}