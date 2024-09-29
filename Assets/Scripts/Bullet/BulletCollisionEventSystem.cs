using System.ComponentModel;
using Bullet;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Random = UnityEngine.Random;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletCollisionEventSystem : JobComponentSystem
{
    private BuildPhysicsWorld _physicsWorld;
    private StepPhysicsWorld _stepWorld;

    protected override void OnCreate()
    {
        _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct CollisionJobEvent : ICollisionEventsJob
    {
        [ReadOnly(true)] public ComponentDataFromEntity<BulletData> BulletGroup;
        [ReadOnly(true)] public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.Entities.EntityA;
            var entityB = collisionEvent.Entities.EntityB;

            var isTargetA = PhysicsVelocityGroup.Exists(entityA);
            var isTargetB = PhysicsVelocityGroup.Exists(entityB);

            var isBulletA = BulletGroup.Exists(entityA);
            var isBulletB = BulletGroup.Exists(entityB);

            if (isBulletA && isTargetB)
            {
                var velocityComponent = PhysicsVelocityGroup[entityB];
                velocityComponent.Linear = new float3(0, 1000, 0);
                PhysicsVelocityGroup[entityB] = velocityComponent;
            }

            if (isBulletB && isTargetA)
            {
                var velocityComponent = PhysicsVelocityGroup[entityA];
                velocityComponent.Linear = new float3(0, 1000, 0);
                PhysicsVelocityGroup[entityA] = velocityComponent;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var scaling = Random.RandomRange(1.0f, 5.0f);
        
        var jobHandle1 = new CollisionJobEvent
        {
            BulletGroup = GetComponentDataFromEntity<BulletData>(),
            PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>()
        }.Schedule(_stepWorld.Simulation, ref _physicsWorld.PhysicsWorld, inputDeps);

        jobHandle1.Complete();
        // var jobHandle2 = Entities.WithoutBurst().WithName("BulletCollisionEventSystem").WithStructuralChanges()
        //     .ForEach((ref NonUniformScale scale, ref Translation pos) =>
        //     {
        //         scale = new NonUniformScale() { Value = scaling };
        //         pos.Value = new float3(0,0,0);
        //     }).Schedule(inputDeps);
        return jobHandle1;
    }
}