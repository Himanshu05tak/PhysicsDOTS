using Bullet;
using Unity.Jobs;
using UnityEngine;
using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Character
{
    public class CharacterControllerSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            var inputY = Input.GetAxis("Horizontal");
            var inputZ = Input.GetAxis("Vertical");
            var shooting = Input.GetAxis("Fire1");

            var jobHandle = Entities.WithName("CharacterControllerSystem").ForEach(
                (ref PhysicsVelocity physics, ref Rotation rot, ref CharacterData characterData) =>
                {
                    if (inputZ == 0)
                        physics.Linear = float3.zero;
                    else
                        physics.Linear += inputZ * (characterData.MoveSpeed * deltaTime) * math.forward(rot.Value);
                    //physics.Angular = new float3(0,inputY * characterData.RotationalSpeed, 0);
                    rot.Value = math.mul(math.normalize(rot.Value),
                        quaternion.AxisAngle(math.up(), inputY * deltaTime * characterData.RotationalSpeed));
                }).Schedule(inputDeps);
            jobHandle.Complete();
            
            Entities.WithBurst().WithStructuralChanges().WithName("ShootControllerSystem").ForEach(
                (ref PhysicsVelocity physics, ref Translation pos, ref Rotation rot, ref CharacterData player) =>
                {
                    if (!(shooting > 0)) return;
                    var instance = EntityManager.Instantiate(player.Bullet);
                    var offset = new float3(Random.Range(-0.5f, 0.5f), 1f, 1f);
                    EntityManager.SetComponentData(instance,
                        new Translation { Value = pos.Value + math.mul(rot.Value, offset) });
                    EntityManager.SetComponentData(instance, new Rotation{Value = rot.Value});

                    var rSpeed = Random.Range(20, 150);
                    var rEffect = new float3(Random.Range(-500, 500), Random.Range(0, 500), Random.Range(-500, 500));
                    EntityManager.SetComponentData(instance,
                        new BulletData { MoveSpeed = rSpeed, CollisionEffect = rEffect });
                }).Run();
            return jobHandle;
        }
    }
}