using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Lono.Data
{
    public class Entity
    {
        public Dictionary<string, List<Component>> Components { get; set; } = new Dictionary<string, List<Component>>();
        public Dictionary<string, Component> ComponentsByID { get; set; } = new Dictionary<string, Component>();

        [JsonIgnore]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        public void AddComponent(Component component)
        {
            string componentType = component.GetTypeIdentifier();

            foreach (string dependency in component.GetDependencies())
            {
                if (!HasComponent(dependency)) throw new InvalidOperationException($"Attempted to add component of type '{componentType}' to entity {ID}, but entity does not have dependency of type '{dependency}'");
            }

            if (Components.ContainsKey(componentType))
            {
                if (component.GetIsUnique()) throw new InvalidOperationException($"Attempted to add unique component of type '{componentType}' to entity {ID}, but a component of that type already exists.");
                Components[componentType].Add(component);
            }
            else
            {
                Components.Add(componentType, new List<Component>() { component });
            }
            ComponentsByID.Add(component.ID, component);
        }

        public bool HasComponent(string componentType)
        {
            return Components.ContainsKey(componentType);
        }

        public List<Component> GetComponents(string componentType)
        {
            if (!Components.ContainsKey(componentType)) return null;

            return Components[componentType].ToList();
        }
        public List<T> GetComponents<T>(string componentType) where T : Component
        {
            if (!Components.ContainsKey(componentType)) return null;

            return Components[componentType].Cast<T>().ToList();
        }

        public Component GetComponent(string componentID)
        {
            if (!ComponentsByID.ContainsKey(componentID)) return null;

            return ComponentsByID[componentID];
        }
        public T GetComponent<T>(string componentID) where T : Component
        {
            if (!ComponentsByID.ContainsKey(componentID)) return null;

            return (T)ComponentsByID[componentID];
        }
    }
}
