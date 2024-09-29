using Unity.Entities;

namespace Duck
{
     [GenerateAuthoringComponent]
    public struct DuckData : IComponentData
    {
        public bool ShouldDestroy;
    }
}
