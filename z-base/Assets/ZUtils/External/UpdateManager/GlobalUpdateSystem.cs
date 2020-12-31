using System;
using System.Collections.Generic;

namespace Zitga.Update
{
    public class GlobalUpdateSystem : IUpdateSystem
    {
        private readonly List<IUpdateSystem> updates;

        /// <summary>
        /// Current number update function
        /// </summary>
        public int Count
        {
            get { return updates.Count; }
        }

        public GlobalUpdateSystem()
        {
            updates = new List<IUpdateSystem>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (IUpdateSystem update in updates)
            {
                update?.OnUpdate(deltaTime);
            }
        }

        /// <summary>
        /// Add update function from someone object what need to call
        /// </summary>
        /// <param name="update"></param>
        /// <exception cref="Exception"></exception>
        public void Add(IUpdateSystem update)
        {
            if (updates.Contains(update))
            {
                throw new Exception($"Object is exist in updates: {nameof(update)}");
            }

            updates.Add(update);
        }

        /// <summary>
        /// remove update from a object when it does not use
        /// </summary>
        /// <param name="update"></param>
        /// <exception cref="Exception"></exception>
        public void Remove(IUpdateSystem update)
        {
            if (updates.Contains(update))
            {
                updates.Remove(update);
            }
            else
            {
                throw new Exception($"Object is not exist in updates: {nameof(update)}");
            }
        }
    }
}