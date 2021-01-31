using Newtonsoft.Json;

using Lono.Data;

namespace Lono
{
    public static class Serialization
    {
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static string SerializeEntity(Entity entity)
        {
            return JsonConvert.SerializeObject(entity, jsonSettings);
        }
        public static string SerializeComponent(Component component)
        {
            return JsonConvert.SerializeObject(component, jsonSettings);
        }

        public static Entity DeserializeEntity(string json)
        {
            return JsonConvert.DeserializeObject<Entity>(json, jsonSettings);
        }
        public static T DeserializeComponent<T>(string json) where T : Component
        {
            return JsonConvert.DeserializeObject<T>(json, jsonSettings);
        }
    }
}
