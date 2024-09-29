using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Jobs;

namespace Duck
{
    public class SpawnManagerSystem : JobComponentSystem
    {
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // var jobHandle = Entities.WithName("SpawnManagerSystem").ForEach(() =>
            // {
            //     
            // }).Schedule(inputDeps);
            return inputDeps;
        }
    }
}
