using System;

namespace JsonStore
{
    public class DocumentState<TDocument, TId, TContent>
        where TContent : class
        where TDocument : Document<TContent, TId>
    {
        public DocumentState(DocumentStates currentState, TDocument document)
        {
            CurrentState = currentState;
            Document = document ?? throw new ArgumentException("The document cannot be null when creating a document state.", nameof(document));
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