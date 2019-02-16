using System;

namespace JsonStore
{
    public abstract class Document<TContent, TId> where TContent : class
    {
        protected Document()
        {
            
        }

        protected Document(TContent content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public TId Id => GetId();
        public TContent Content { get; internal set; }

        protected abstract TId GetId();
    }

    public abstract class Document<TContent> : Document<TContent, string>
        where TContent : class
    {
        protected Document(TContent content)
            : base(content)
        {
        }

        protected Document()
            : base()
        {
        }
    }
}