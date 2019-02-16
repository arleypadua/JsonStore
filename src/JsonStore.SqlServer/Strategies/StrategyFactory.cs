using System;

namespace JsonStore.SqlServer.Strategies
{
    internal class StrategyFactory
    {
        internal static Strategy GetStrategy<TDocument, TId, TContent>(
            Collection<TDocument, TId, TContent> collectionInstance,
            DocumentState<TDocument, TId, TContent> documentState)
            where TContent : class
            where TDocument : Document<TContent, TId>, new()
        {
            switch (documentState.CurrentState)
            {
                case DocumentStates.Modified:
                    return new UpdateStrategy<TDocument, TId, TContent>(collectionInstance, documentState.Document);
                case DocumentStates.Added:
                    return new InsertStrategy<TDocument, TId, TContent>(collectionInstance, documentState.Document);
                case DocumentStates.Removed:
                    return new RemoveStrategy<TDocument, TId, TContent>(collectionInstance, documentState.Document);
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentState), documentState, null);
            }
        }
    }
}