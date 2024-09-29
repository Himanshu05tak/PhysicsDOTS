using Unity.Entities;
using Unity.Jobs;

namespace Bullet
{
    public class ShootControllerSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return inputDeps;
        }
    }
}
