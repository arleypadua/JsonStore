using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JsonStore.Sql.LocalDbTests.Documents;
using JsonStore.Sql.LocalDbTests.Model;
using Xunit;
using Xunit.Abstractions;

namespace JsonStore.Sql.LocalDbTests
{
    public class LoadTests : IClassFixture<DatabaseFixture>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly PersonCollection _collection;

        public LoadTests(DatabaseFixture fixture,
            ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _collection = new PersonCollection(fixture.Connection);
        }

        [Fact]
        public async Task Insert1KRecords()
        {
            var amountOfPeople = 100000;
            var random = new Random();
            var sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < amountOfPeople; i++)
            {
                _collection.Add(Person.Create(random.Next(1, 99), Guid.NewGuid().ToString()));
            }

            _outputHelper.WriteLine("Added {0} people. {1} seconds", amountOfPeople, sw.Elapsed.TotalSeconds);

            sw.Stop();

            Assert.InRange(sw.Elapsed.TotalMilliseconds, 0, 2000);

            sw.Restart();

            await _collection.CommitAsync();

            _outputHelper.WriteLine("Created {0} people in the database. {1} seconds", amountOfPeople, sw.Elapsed.TotalSeconds);

            sw.Stop();

            Assert.InRange(sw.Elapsed.TotalSeconds, 0, 40);

            Assert.Empty(_collection.GetModifiedDocuments());
        }
    }
}