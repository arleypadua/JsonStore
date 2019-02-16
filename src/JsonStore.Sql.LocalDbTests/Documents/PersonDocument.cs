using JsonStore.Sql.LocalDbTests.Model;

namespace JsonStore.Sql.LocalDbTests.Documents
{
    public class PersonDocument : Document<Person>
    {
        protected override string GetId() => Content.Id;
    }
}