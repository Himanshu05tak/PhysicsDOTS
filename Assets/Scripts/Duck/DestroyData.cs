using Unity.Entities;

namespace Duck
{
     [GenerateAuthoringComponent]
    public struct DestroyData : IComponentData
    {
        public bool ShouldDestroy;
    }
}
