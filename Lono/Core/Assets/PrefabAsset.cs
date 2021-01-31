using System.IO;
using System.Linq;

using Lono.Data;
using Lono.Core.Components;

namespace Lono.Core.Assets
{
    public class PrefabAsset : Asset
    {
        public string Json { get; set; }

        public PrefabAsset(string name) : base(name) { }

        public override void LoadFromFile(Stream fileStream)
        {
            using (StreamReader sr = new StreamReader(fileStream))
            {
                Json = sr.ReadToEnd().Trim();
            }
        }

        public Entity InstantiatePrefab(params Component[] components)
        {
            Entity entity = Serialization.DeserializeEntity(Json);
            foreach (Component c in components) entity.AddComponent(c);
            return entity;
        }
        public Entity InstantiatePrefab(Vector2 position, params Component[] components)
        {
            Entity entity = InstantiatePrefab(components);
            var transform = entity.GetComponents<TransformComponent>("core_transform").First();
            transform.Position = position;
            return entity;
        }
    }
}
