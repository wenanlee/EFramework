// SystemBase.cs
using System;
namespace EFramework.Unity.ECS
{

    [Serializable]
    public abstract class SystemBase
    {
        public abstract void Setup(GameEntity entity);
        public abstract void Update(GameEntity entity);
    }
}