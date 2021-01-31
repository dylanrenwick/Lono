using System;

using Newtonsoft.Json;

namespace Lono.Data
{
    public abstract class Component
    {
        [JsonIgnore]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        public abstract string GetTypeIdentifier();

        public virtual string[] GetDependencies() => new string[0];
        public virtual bool GetIsUnique() => false;
    }
}
