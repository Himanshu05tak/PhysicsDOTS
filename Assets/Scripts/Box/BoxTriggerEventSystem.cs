using Unity.Jobs;
using Unity.Physics;
using Unity.Entities;
using Unity.Collections;
using Unity.Physics.Systems;

namespace Box
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class BoxTriggerEventSystem : JobComponentSystem
    {
        private BuildPhysicsWorld _physicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnCreate()
        {
            _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        struct BoxTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<BoxTriggerData> BoxTriggerDataGroup;
            public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                var entityA = triggerEvent.Entities.EntityA;
                var entityB = triggerEvent.Entities.EntityB;

                var isBodyATrigger = BoxTriggerDataGroup.Exists(entityA);
                var isBodyBTrigger = BoxTriggerDataGroup.Exists(entityB);

                if (isBodyATrigger && isBodyBTrigger) return;
                
                var isBodyADynamic = PhysicsVelocityGroup.Exists(entityA);
                var isBodyBDynamic = PhysicsVelocityGroup.Exists(entityB);

                if ((isBodyATrigger && !isBodyBDynamic) || isBodyBTrigger && !isBodyADynamic) return;

                var triggerEntity = isBodyATrigger ? entityA : entityB;
                var dynamicEntity = isBodyATrigger ? entityB : entityA;

                var component = PhysicsVelocityGroup[dynamicEntity];
                var boxTrigger = BoxTriggerDataGroup[triggerEntity];
                component.Linear += boxTrigger.TriggerEffect;
                PhysicsVelocityGroup[dynamicEntity] = component;
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobHandle = new BoxTriggerJob
            {
                BoxTriggerDataGroup = GetComponentDataFromEntity<BoxTriggerData>(),
                PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>()
            }.Schedule(_stepPhysicsWorld.Simulation, ref _physicsWorld.PhysicsWorld,inputDeps);
            
            return jobHandle;
        }
    }
}
