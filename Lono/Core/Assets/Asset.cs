using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lono.Core.Assets
{
    public abstract class Asset
    {
        public string Name { get; private set; }

        public Asset(string name)
        {
            Name = name;
        }

        public abstract void LoadFromFile(Stream fileStream);
    }
}
