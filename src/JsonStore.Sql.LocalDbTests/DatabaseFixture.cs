using System;
using System.Data.SqlClient;
using Dapper;
using JsonStore.Sql.LocalDbTests.Documents;
using JsonStore.Sql.LocalDbTests.Model;

namespace JsonStore.Sql.LocalDbTests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JsonStoreTestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            Connection.Open();

            Connection.Execute(
                @"IF EXISTS (select * from sysobjects where name='Person' and xtype='U')
                        DROP TABLE Person");

            Connection.Execute(
                @"CREATE TABLE Person
                    (
	                    [_Id] varchar(50) primary key,
	                    [_Document] varchar(max) not null,
	                    [Age] int not null,
                        [Name] varchar(250) not null
                    )");

            var seedDataCollection = new PersonCollection(Connection);

            seedDataCollection.Add(Mary);
            seedDataCollection.Add(John);

            seedDataCollection.CommitAsync().GetAwaiter().GetResult();
        }
        
        public Person Mary = Person.Create(25, "Mary");
        public Person John = Person.Create(29, "John");
        public string Test() => "";
        public void Dispose()
        {
            Connection.Execute(
                @"IF EXISTS (select * from sysobjects where name='Person' and xtype='U')
                        DROP TABLE Person");

            Connection.Dispose();
        }

        public SqlConnection Connection { get; private set; }
    }
}