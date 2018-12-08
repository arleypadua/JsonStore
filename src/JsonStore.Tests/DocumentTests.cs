using System;
using Xunit;

namespace JsonStore.Tests
{
    public class DocumentTests
    {
        [Fact]
        public void Document_WithConventionId_WhenAccessingId_ShouldMatchTheContent()
        {
            string id = "doc-id";

            var myContent = new TestContentStringId
            {
                Id = id,
                AnyNumber = 5
            };

            var myDoc = new Document<TestContentStringId>(myContent);

            Assert.Equal(id, myDoc.Id);
        }

        [Fact]
        public void Document_WithoutMatchingIdTypes_WhenAccessingId_ShouldThrowException()
        {
            string id = "doc-id";

            var myContent = new TestContentStringId
            {
                Id = id,
                AnyNumber = 5
            };

            var myDoc = new Document<TestContentStringId, Guid>(myContent);

            Assert.Throws<InvalidOperationException>(() => myDoc.Id);
        }

        [Fact]
        public void Document_WithoutId_DefiningHowToGetId_WhenAccessingId_ShouldMatchTheContent()
        {
            string id = "doc-id";

            var myContent = new TestContentStringWithoutConventions
            {
                MyId = id,
                AnyNumber = 5
            };

            var myDoc = new TestDocument(myContent);

            Assert.Equal(id, myDoc.Id);
        }

        class TestContentStringId
        {
            public string Id { get; set; }
            public int AnyNumber { get; set; }
        }

        class TestContentStringWithoutConventions
        {
            public string MyId { get; set; }
            public int AnyNumber { get; set; }
        }

        class TestDocument : Document<TestContentStringWithoutConventions>
        {
            public TestDocument(TestContentStringWithoutConventions content) 
                : base(content)
            {
            }

            protected override string GetId() => Content.MyId;
        }
    }
}
