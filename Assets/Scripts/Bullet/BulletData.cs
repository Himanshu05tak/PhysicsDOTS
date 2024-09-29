using Unity.Entities;
using Unity.Mathematics;

namespace Bullet
{
    [GenerateAuthoringComponent]
    public struct BulletData : IComponentData
    {
        public float MoveSpeed;
        public float3 CollisionEffect;
    }
}
