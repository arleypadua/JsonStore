using System;

namespace JsonStore
{
    public class Document<TContent, TId> where TContent : class
    {
        public Document()
        {
            
        }

        public Document(TContent contentWithoutConventionId)
        {
            Content = contentWithoutConventionId ?? throw new ArgumentNullException(nameof(contentWithoutConventionId));
        }

        public TId Id => GetId();
        public TContent Content { get; internal set; }

        protected virtual TId GetId()
        {
            var idPropertyByConvention = typeof(TContent).GetProperty("Id");

            if (idPropertyByConvention?.GetMethod == null)
            {
                throw new InvalidOperationException("The content needs to provide a property named Id or configuring how to get the Id by overriding the method 'GetId'.");
            }

            if (idPropertyByConvention.PropertyType != typeof(TId))
            {
                throw new InvalidOperationException("In order to use the default Id convention, the type of the property on the content needs to match the same as in the document.");
            }

            return (TId)idPropertyByConvention.GetMethod.Invoke(Content, null);
        }
    }

    public class Document<TContent> : Document<TContent, string>
        where TContent : class
    {
        public Document()
        {
        }

        public Document(TContent content) 
            : base(content)
        {
        }
    }
}