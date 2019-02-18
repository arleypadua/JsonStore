# JsonStore

JsonStore is a lightweight layer of abstraction of documents that can be applied on top of your classes. It is mainly focused for relational databases, but can be extensible to any sotrage technology.

It also works as a micro-orm when using with the **JsonStore.Sql** extension package.

## Getting started

1) Add a NuGet reference
```powershell
PM> Install-Package JsonStore.Sql
```

Given the following class to be stored
```csharp
public class Foo
{
    public string Id { get; set; }
    public string AnyProperty { get; set; }
}
```

2) You will need a **Document** representation of Foo
```csharp
public class FooDocument : Document<Foo>
{
    protected override string GetId() => Content.Id;
}
```

3) And a **Collection** of Foo documents:
```csharp
public class FooCollection : Collection<FooDocument, Foo>
{
    public FooCollection(IStoreDocuments documentsStore) 
        : base(documentsStore)
    {
    }
}
```

A collection can also be seen as an unit of work. It is responsible to track the state of documents locally and has a `Commit` method that persists everything at the end of your process.

Optionally you can configure more fields that will be stored as columns in the database by overriding the method `GetIndexedValuesInternal`:

```csharp
protected override IReadOnlyDictionary<string, object> GetIndexedValuesInternal(FooDocument document) => new Dictionary<string, object>
{
    [nameof(Foo.AnyProperty)] = document.Content.AnyProperty
};
```

Notice that the collection requires an instance of `IStoreDocuments`. The dependency injection configured on the 4th step will be able to provide an instance of it, but you can also implement one yourself.

```
NOTE: the default implementation of IStoreDocuments is an instance of SqlServerDocumentStore.
```

4) In the **Startup.cs** class, add the following line at the the end of ConfigureServices method:
```csharp
public class Startup
{
    // ... 

    public void ConfigureServices(IServiceCollection services)
    {
        // prior configuration here...

        services.AddSqlServerJsonStore("CONNECTION_STRING");
        services.AddScoped<FooCollection>();
    }
}
```

5) And then the usage:
```csharp
[Route("api/[controller]")]
public class FooController : Controller
{
    private readonly FooCollection _collection;
    public FooController(FooCollection collection)
    {
        _collection = collection;
    }

    [HttpPost]
    public Task PostAsync([FromBody]Foo foo)
    {
        _collection.Add(foo);
        return _collection.CommitAsync();
    }

    [HttpGet("{id}")]
    public async Task<Foo> GetAsync(string id)
    {
        var fooDocument = await _collection.GetFromStore(id);
        return fooDocument.Content;
    }
}
```

6) Finally create the database structure:
```sql
CREATE TABLE Foo
(
    [_Id] varchar(50) primary key,
    [_Document] varchar(max) not null,
    -- [AnyProperty] varchar(250) -- optional field as suggested on step 3
)
```

```
NOTE: the library still doesn't supports script generation or migrations. Therefore you need to provide them every time need a change.
```