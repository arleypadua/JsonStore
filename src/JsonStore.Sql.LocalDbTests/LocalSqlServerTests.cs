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
        public async Task NonExistingPerson_WhenAdding_ThenPersonSuccessfulyRestoredFromDatabase()
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

        [Fact]
        public async Task ExistingPerson_WhenChangingName_NameShouldBePersisted()
        {
            var mary = await _collection.GetFromStore(_fixture.Mary.Id);

            mary.Content.ChangeName("Mary Mae");

            _collection.MarkAsModified(mary.Id);

            await _collection.CommitAsync();

            var maryFromDb = await _collection.GetFromStore(mary.Id);

            Assert.Equal(mary.Content.Id, maryFromDb.Content.Id);
            Assert.Equal(mary.Content.Age, maryFromDb.Content.Age);
            Assert.Equal(mary.Content.Name, maryFromDb.Content.Name);
        }

        [Fact]
        public async Task ExistingPerson_WhenDeleting_RecordShouldBeDeleted()
        {
            var john = await _collection.GetFromStore(_fixture.John.Id);
            
            _collection.MarkAsRemoved(john.Id);

            await _collection.CommitAsync();

            var maryFromDb = await _collection.GetFromStore(john.Id);

            Assert.Null(maryFromDb);
        }
    }
}
