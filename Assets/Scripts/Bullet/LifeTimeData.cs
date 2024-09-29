using Unity.Entities;

namespace Bullet
{
    [GenerateAuthoringComponent]
    public struct LifeTimeData : IComponentData
    {
        public float timeLeft;
    }
}
