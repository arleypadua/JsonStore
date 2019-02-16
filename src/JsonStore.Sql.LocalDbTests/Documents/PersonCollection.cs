using System.Collections.Generic;
using System.Data;
using JsonStore.Abstractions;
using JsonStore.Sql.LocalDbTests.Model;

namespace JsonStore.Sql.LocalDbTests.Documents
{
    public class PersonCollection : Collection<PersonDocument, Person>
    {
        public PersonCollection(IDbConnection connection) 
            : base(new SqlServerDocumentStore(connection))
        {
        }

        protected override IReadOnlyDictionary<string, object> GetIndexedValuesInternal(PersonDocument document)
        {
            return new Dictionary<string, object>
            {
                [nameof(Person.Age)] = document.Content.Age,
                [nameof(Person.Name)] = document.Content.Name,
            };
        }
    }
}