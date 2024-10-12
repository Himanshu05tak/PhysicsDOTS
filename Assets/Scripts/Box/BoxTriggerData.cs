using Unity.Entities;
using Unity.Mathematics;

namespace Box
{
    [GenerateAuthoringComponent]
    public struct BoxTriggerData : IComponentData
    {
        public float3 TriggerEffect;
    }
}
