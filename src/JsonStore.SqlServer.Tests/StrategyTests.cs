using JsonStore.Abstractions;
using JsonStore.SqlServer.Strategies;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace JsonStore.SqlServer.Tests
{
    public class StrategyTests
    {
        private readonly IStoreDocuments _documentsStore = new Mock<IStoreDocuments>().Object;

        [Fact]
        public void InsertStrategyWithDocument_WhenGettingCommand_SqlCommandShouldBeExpected()
        {
            var collection = new TestCollection(_documentsStore);
            var document = collection.TrackDocumentFromJsonContent("{ \"Id\":\"id\", \"MyNumber\":5 }");

            var strategy = new InsertStrategy<TestDocument, string, TestContent>(collection, document);
            var command = strategy.GetCommand();

            Assert.Equal("INSERT INTO [TestContent] ([_Document], [_Id], [MyNumber]) VALUES ('{\"Id\":\"id\",\"MyNumber\":5}', 'id', 5)", command.ToString());
        }

        [Fact]
        public void UpdateStrategyWithDocument_WhenGettingCommand_SqlCommandShouldBeExpected()
        {
            var collection = new TestCollection(_documentsStore);
            var document = collection.TrackDocumentFromJsonContent("{ \"Id\":\"id\", \"MyNumber\":5 }");

            document.Content.ChangeNumber(10);

            var strategy = new UpdateStrategy<TestDocument, string, TestContent>(collection, document);
            var command = strategy.GetCommand();

            Assert.Equal("UPDATE [TestContent] SET [_Document] = '{\"Id\":\"id\",\"MyNumber\":10}', [MyNumber] = 10 WHERE [_Id] = 'id'", command.ToString());
        }

        [Fact]
        public void RemoveStrategyWithDocument_WhenGettingCommand_SqlCommandShouldBeExpected()
        {
            var collection = new TestCollection(_documentsStore);
            var document = collection.TrackDocumentFromJsonContent("{ \"Id\":\"id\", \"MyNumber\":5 }");

            var strategy = new RemoveStrategy<TestDocument, string, TestContent>(collection, document);
            var command = strategy.GetCommand();

            Assert.Equal("DELETE FROM [TestContent] WHERE [_Id] = 'id'", command.ToString());
        }

        class TestCollection : Collection<TestDocument, TestContent>
        {
            public TestCollection(IStoreDocuments documentsStore) 
                : base(documentsStore)
            {
            }

            protected override IReadOnlyDictionary<string, object> GetIndexedValuesInternal(TestDocument document)
            {
                return new Dictionary<string, object>
                {
                    [nameof(TestContent.MyNumber)] = document.Content.MyNumber
                };
            }
        }

        class TestContent
        {
            public TestContent(string id, int myNumber)
            {
                Id = id ?? throw new ArgumentNullException(nameof(id));
                MyNumber = myNumber;
            }

            public string Id { get; private set; }
            public int MyNumber { get; private set; }

            public void ChangeNumber(int number)
            {
                MyNumber = number;
            }
        }

        class TestDocument : Document<TestContent>
        {
            public TestDocument(TestContent content)
                : base(content)
            {

            }

            public TestDocument()
            {

            }

            protected override string GetId()
            {
                return Content.Id;
            }
        }
    }
}
