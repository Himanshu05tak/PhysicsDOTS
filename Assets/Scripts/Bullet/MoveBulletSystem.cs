using Unity.Jobs;
using Unity.Physics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Bullet
{
   public class MoveBulletSystem : JobComponentSystem
   {
      protected override JobHandle OnUpdate(JobHandle inputDeps)
      {
         var deltaTime = Time.DeltaTime;
         var jobHandle = Entities.WithName("MoveBulletSystem").ForEach((ref PhysicsVelocity physics, ref Translation pos, ref Rotation rot, ref BulletData bulletData) =>
         {
            physics.Angular = float3.zero;
            physics.Linear += bulletData.MoveSpeed * deltaTime * math.forward(rot.Value);
         }).Schedule(inputDeps);
         
         jobHandle.Complete();
         return inputDeps;
      }
   }
}
