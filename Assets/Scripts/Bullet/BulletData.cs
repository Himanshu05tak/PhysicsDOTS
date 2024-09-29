using Unity.Entities;

namespace Bullet
{
    [GenerateAuthoringComponent]
    public struct BulletData : IComponentData
    {
        public float MoveSpeed;
        public Entity Explosion;
    }
}
