using Unity.Entities;

namespace Character
{
    [GenerateAuthoringComponent]
    public struct CharacterData : IComponentData
    {
        public float MoveSpeed;
        public float RotationalSpeed;
        public Entity Bullet;
    }
}
