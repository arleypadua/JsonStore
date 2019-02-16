using System.Threading.Tasks;

namespace JsonStore.Abstractions
{
    public interface IStoreDocuments
    {
        Task StoreAsync<TDocument, TId, TContent>(Collection<TDocument, TId, TContent> collection)
            where TContent : class
            where TDocument : Document<TContent, TId>, new();

        Task<string> GetDocumentContentById<TDocument, TId, TContent>(Collection<TDocument, TId, TContent> collection, TId id)
            where TContent : class
            where TDocument : Document<TContent, TId>, new();
    }
}