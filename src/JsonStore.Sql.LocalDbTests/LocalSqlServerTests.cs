using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using JsonStore.Sql.LocalDbTests.Documents;
using JsonStore.Sql.LocalDbTests.Model;
using Xunit;

namespace JsonStore.Sql.LocalDbTests
{
    public class LocalSqlServerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private PersonCollection _collection;

        public LocalSqlServerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _collection = new PersonCollection(_fixture.Connection);
        }

        [Fact]
        public async Task CreatedPerson_WhenAdding_ThenOperationShouldSucceed()
        {
            var carl = Person.Create(19, "Carl");

            _collection.Add(carl);

            await _collection.CommitAsync();
            Assert.Empty(_collection.GetModifiedDocuments());

            var carlFromDb = await _collection.GetFromStore(carl.Id);
            
            Assert.Equal(carl.Id, carlFromDb.Content.Id);
            Assert.Equal(carl.Age, carlFromDb.Content.Age);
            Assert.Equal(carl.Name, carlFromDb.Content.Name);
        }
    }
}
