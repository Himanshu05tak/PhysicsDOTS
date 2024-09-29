using Duck;
using Unity.Jobs;
using Unity.Physics;
using Unity.Entities;
using Unity.Physics.Systems;
using System.ComponentModel;

namespace Bullet
{
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
            [ReadOnly(true)] public ComponentDataFromEntity<DuckData> DuckDataGroup;

            public void Execute(CollisionEvent collisionEvent)
            {
                var entityA = collisionEvent.Entities.EntityA;
                var entityB = collisionEvent.Entities.EntityB;

                var isTargetA = DuckDataGroup.Exists(entityA);
                var isTargetB = DuckDataGroup.Exists(entityB);

                var isBulletA = BulletGroup.Exists(entityA);
                var isBulletB = BulletGroup.Exists(entityB);

                if (isBulletA && isTargetB)
                {
                    var destroyComponent = DuckDataGroup[entityB];
                    destroyComponent.ShouldDestroy = true;
                    DuckDataGroup[entityB] = destroyComponent;
                }

                if (isBulletB && isTargetA)
                {
                    var destroyComponent = DuckDataGroup[entityA];
                    destroyComponent.ShouldDestroy = true;
                    DuckDataGroup[entityA] = destroyComponent;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobHandle1 = new CollisionJobEvent
            {
                BulletGroup = GetComponentDataFromEntity<BulletData>(),
                DuckDataGroup = GetComponentDataFromEntity<DuckData>()
            }.Schedule(_stepWorld.Simulation, ref _physicsWorld.PhysicsWorld, inputDeps);

            jobHandle1.Complete();
            return jobHandle1;
        }
    }
}