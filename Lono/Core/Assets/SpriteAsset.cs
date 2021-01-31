using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Lono.Data;
using Lono.BigGustave;

namespace Lono.Core.Assets
{
    public class SpriteAsset : Asset
    {
        public byte[] Data { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Vector2 Size => new Vector2(Width, Height);

        public SpriteAsset(string name): base(name) { }

        public override void LoadFromFile(Stream fileStream)
        {
            Png png = Png.Open(fileStream);
            Width = png.Width;
            Height = png.Height;
            Data = png.RawData.GetRawPixelData();
        }
    }
}
