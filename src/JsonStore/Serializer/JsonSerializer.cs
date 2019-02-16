using Newtonsoft.Json;

namespace JsonStore.Serializer
{
    public class JsonSerializer : ISerializer
    {
        private static readonly PrivateSetterContractResolver ContractResolver = new PrivateSetterContractResolver();
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = ContractResolver
        };

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, Settings);
        }
    }
}