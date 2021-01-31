using System;
using System.IO;
using System.Collections.Generic;

using Lono.Core.Assets;
using Lono.Data;

namespace Lono
{
    public class LonoGame
    {
        private Dictionary<string, Asset> assets = new Dictionary<string, Asset>();
        private IRenderWrapper renderWrapper;
        private IInputWrapper inputWrapper;

        private Vector2 screenSize;
        public double ScreenWidth { get => screenSize.X; }
        public double ScreenHeight { get => screenSize.Y; }

        private Scene activeScene;
        public Scene ActiveScene
        {
            get => activeScene;
            set
            {
                if (activeScene != null) activeScene.TearDown();
                activeScene = value;
                activeScene.Initialize(this);
                activeScene.Setup();
            }
        }

        private string assetsPath;
        public string AssetsPath
        {
            get => assetsPath;
            set
            {
                assetsPath = value;
                if (assetsPath.EndsWith("\\") || assetsPath.EndsWith("/"))
                {
                    assetsPath = assetsPath.Substring(0, assetsPath.Length - 1);
                }
            }
        }

        public LonoGame(Vector2 size, IRenderWrapper renderer, IInputWrapper input)
        {
            screenSize = size;
            renderWrapper = renderer;
            inputWrapper = input;
        }

        public void Update(TimeSpan deltaTime)
        {
            inputWrapper.Update();
            ActiveScene.Update(deltaTime, renderWrapper, inputWrapper);
        }

        public void LoadAsset<T>(string name, string filePath) where T : Asset
        {
            Asset newAsset = (Asset)Activator.CreateInstance(typeof(T), new object[] { name });

            string fullPathToFile = $"{AssetsPath}\\{filePath}";
            if (!File.Exists(fullPathToFile)) throw new ArgumentException($"File not found: '{fullPathToFile}'");
            using (var fileStream = File.OpenRead(fullPathToFile))
            {
                newAsset.LoadFromFile(fileStream);
            }

            assets.Add(name, newAsset);
        }

        public T GetAsset<T>(string name) where T : Asset
        {
            if (!assets.ContainsKey(name)) throw new ArgumentException($"Asset {name} was not loaded!");
            Asset asset = assets[name];
            return (T)asset;
        }
    }
}
