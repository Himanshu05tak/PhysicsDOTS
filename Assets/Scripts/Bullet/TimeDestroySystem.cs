using Unity.Jobs;
using Unity.Entities;

namespace Bullet
{
    public class TimeDestroySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            Entities.WithoutBurst().WithStructuralChanges().WithName("TimeDestroySystem").ForEach(
                (Entity entity, ref LifeTimeData lifeTimeData) =>
                {
                    lifeTimeData.timeLeft -= deltaTime;
                    if (lifeTimeData.timeLeft <= 0f)
                        EntityManager.DestroyEntity(entity);
                }).Run();
            return inputDeps;
        }
    }
}