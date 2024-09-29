using System;
using Bullet;
using Character;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace ECSManager
{
    public class ECSManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bulletPrefab;
        
        private EntityManager _entityManager;
        private BlobAssetStore _store;
        private void Start()
        {
            _store = new BlobAssetStore();
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
            var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
            var bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

            var playerCharacter = _entityManager.Instantiate(playerEntity);
            _entityManager.SetComponentData(playerCharacter,
                new Translation() { Value = new float3(0,2.2f,0)});
            _entityManager.SetComponentData(playerCharacter,
                new CharacterData { MoveSpeed = 5, RotationalSpeed = 1, Bullet = bulletEntity });
        }

        private void OnDestroy()
        {
            _store.Dispose();
        }
    }
}
