using System;
using Ardalis.GuardClauses;

namespace JsonStore
{
    public class DocumentState<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>
    {
        public DocumentState(DocumentStates currentState, TDocument document)
        {
            Guard.Against.Null(document, nameof(document));

            CurrentState = currentState;
            Document = document;
        }

        public DocumentStates CurrentState { get; internal set; }
        public TDocument Document { get; }
    }

    public enum DocumentStates
    {
        Unmodified,
        Modified,
        Added,
        Removed
    }
}