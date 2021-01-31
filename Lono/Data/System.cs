using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace Lono.Data
{
    public abstract class System
    {
        protected List<string> entities = new List<string>();

        public Scene Scene;

        public virtual void AddEntity(Entity entity)
        {
            entities.Add(entity.ID);
        }

        public abstract bool IsInterestedInEntity(Entity entity);
        public abstract void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input);
    }
}
