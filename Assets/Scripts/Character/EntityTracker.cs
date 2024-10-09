using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Character
{
    public class EntityTracker : MonoBehaviour
    {
        private Entity _entityTracker = Entity.Null;

        public void SetReceivedEntity(Entity entity)
        {
            _entityTracker = entity;
        }

        private void LateUpdate()
        {
            if (_entityTracker != Entity.Null)
            {
                try
                {
                    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                    transform.position = entityManager.GetComponentData<Translation>(_entityTracker).Value;
                    transform.rotation = entityManager.GetComponentData<Rotation>(_entityTracker).Value;
                }
                catch (Exception e)
                {
                    _entityTracker = Entity.Null;
                }
            }
        }
    }
}
