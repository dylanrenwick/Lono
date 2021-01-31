using System;
using System.Linq;
using System.Collections.Generic;

using Lono.Core.Components;

namespace Lono.Data
{
    public abstract class Scene
    {
        private List<System> systems = new List<System>();
        private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

        protected LonoGame game;

        public double ScreenWidth { get => game.ScreenWidth; }
        public double ScreenHeight { get => game.ScreenHeight; }

        public void Initialize(LonoGame game)
        {
            this.game = game;
        }

        public abstract void Setup();

        public virtual void TearDown() { }

        public void AddSystem(System system)
        {
            if (entities.Count > 0) throw new InvalidOperationException("Attempted to add System to scene after adding Entities!");
            system.Scene = this;
            systems.Add(system);
        }

        public void AddSystems(IEnumerable<System> systems)
        {
            foreach (System system in systems) AddSystem(system);
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity.ID, entity);
            foreach (System system in systems)
            {
                if (system.IsInterestedInEntity(entity)) system.AddEntity(entity);
            }
        }

        public void AddEntities(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities) AddEntity(entity);
        }

        public void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            foreach (System system in systems)
            {
                system.Update(deltaTime, renderer, input);
            }
        }

        public Entity GetEntity(string id)
        {
            if (id != null && entities.ContainsKey(id)) return entities[id];

            return null;
        }

        public bool HasEntity(string id)
        {
            return entities.ContainsKey(id);
        }

        public T GetComponent<T>(string id) where T : Component
        {
            var parts = id.Split('.');
            if (parts.Length < 2) return null;
            return GetComponent<T>(parts[0], parts[1]);
        }
        public T GetComponent<T>(string entityID, string componentID) where T : Component
        {
            Entity entity = GetEntity(entityID);
            if (entity == null) return null;

            T component = entity.GetComponent<T>(componentID);
            return component;
        }

        protected Entity CreateEntity(Vector2 position, params Component[] args)
        {
            var list = args.ToList();
            list.Insert(0, new TransformComponent() { Position = position });

            return CreateEntity(list.ToArray());
        }

        protected Entity CreateEntity(params Component[] args)
        {
            var entity = new Entity();

            foreach (Component c in args)
            {
                entity.AddComponent(c);
            }

            return entity;
        }
    }
}
