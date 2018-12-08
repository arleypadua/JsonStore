using System;
using System.Collections.Generic;
using JsonStore.Abstractions;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace JsonStore.Tests
{
    public class CollectionTests
    {
        private IStoreDocuments _documentsStore = new Mock<IStoreDocuments>().Object;
        

        [Fact]
        public void InstantiatedCollection_ShouldHaveItsContentName()
        {
            var collection = new TestCollection(_documentsStore);

            Assert.Equal("TestContent", collection.Name);
        }

        [Fact]
        public void InstantiatedCollection_LoadingFromJson_ShouldReturnMatchingDocument()
        {
            var collection = new TestCollection(_documentsStore);
            var document = collection.TrackDocumentFromJsonContent("{ \"Id\":\"id\", \"AnyNumber\":5 }");

            Assert.Equal("id", document.Content.Id);
            Assert.Equal(5, document.Content.AnyNumber);
            Assert.Equal(document.Id, document.Content.Id);
        }

        [Fact]
        public void InstantiatedCollection_LoadingFromJsonWithAlreadyExistingId_ShouldThrowError()
        {
            void TrackFromJson(TestCollection collectionParam)
            {
                collectionParam.TrackDocumentFromJsonContent("{ \"Id\":\"id\", \"AnyNumber\":5 }");
            }

            var collection = new TestCollection(_documentsStore);
            TrackFromJson(collection);
            
            Assert.Throws<InvalidOperationException>(() => TrackFromJson(collection));
        }

        [Fact]
        public void InstantiatedCollection_AddDocument_ShouldSucceed()
        {
            var collection = new TestCollection(_documentsStore);

            var doc = new Document<TestContent>(new TestContent("id", 5));

            collection.Add(doc);

            Assert.Throws<InvalidOperationException>(() => collection.Add(doc));
        }

        [Fact]
        public void InstantiatedCollectionWithDocument_GetIndexedValuesInternal_ShouldReturnDefaultAndCustom()
        {
            var collection = new TestCollection(_documentsStore);
            var doc = new Document<TestContent>(new TestContent("id", 5));
            collection.Add(doc);

            var indexedValues = collection.GetIndexedValues(doc);

            Assert.True(indexedValues.ContainsKey(Collection.IdKey));
            Assert.True(indexedValues.ContainsKey(Collection.DocumentKey));
            Assert.True(indexedValues.ContainsKey(nameof(TestContent.AnyNumber)));

            Assert.NotNull(indexedValues[Collection.DocumentKey]);
            Assert.NotSame(string.Empty, indexedValues[Collection.DocumentKey]);
        }

        class TestCollection : Collection<Document<TestContent>, TestContent>
        {
            public TestCollection(IStoreDocuments documentsStore) 
                : base(documentsStore)
            {
            }

            public TestCollection(JsonSerializerSettings jsonSerializerSettings, IStoreDocuments documentsStore) 
                : base(jsonSerializerSettings, documentsStore)
            {
            }

            protected override IReadOnlyDictionary<string, object> GetIndexedValuesInternal(Document<TestContent> document)
            {
                return new Dictionary<string, object>
                {
                    ["AnyNumber"] = document.Content.AnyNumber
                };
            }
        }

        class TestContent
        {
            public TestContent(string id, int anyNumber)
            {
                Id = id ?? throw new ArgumentNullException(nameof(id));
                AnyNumber = anyNumber;
            }

            public string Id { get; private set; }
            public int AnyNumber { get; private set; }
        }
    }
}