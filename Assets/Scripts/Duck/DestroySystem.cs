using Unity.Entities;
using Unity.Jobs;

namespace Duck
{
    public class DestroySystem : JobComponentSystem
    {
            protected override JobHandle OnUpdate(JobHandle inputDeps)
            {
                Entities.WithoutBurst().WithStructuralChanges().WithName("DestroySystem").ForEach(
                    (Entity entity, ref DestroyData duckData) =>
                    {
                        if(duckData.ShouldDestroy)
                            EntityManager.DestroyEntity(entity);
                    }).Run();
                return inputDeps;
            }
    }
}
