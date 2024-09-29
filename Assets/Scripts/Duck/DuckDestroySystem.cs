using Unity.Entities;
using Unity.Jobs;

namespace Duck
{
    public class DuckDestroySystem : JobComponentSystem
    {
            protected override JobHandle OnUpdate(JobHandle inputDeps)
            {
                Entities.WithoutBurst().WithStructuralChanges().WithName("DuckDestroySystem").ForEach(
                    (Entity entity, ref DuckData duckData) =>
                    {
                        if(duckData.ShouldDestroy)
                            EntityManager.DestroyEntity(entity);
                    }).Run();
                return inputDeps;
            }
    }
}
