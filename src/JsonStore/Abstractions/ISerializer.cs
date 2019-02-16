namespace JsonStore.Abstractions
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string value);
    }
}