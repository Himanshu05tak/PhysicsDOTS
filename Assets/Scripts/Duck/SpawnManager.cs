using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Duck
{
    public class SpawnManager : MonoBehaviour
    { 
        [SerializeField] private GameObject duckPrefab;

        private const int QUANTITY = 5000;
        private EntityManager _entityManager;
        private BlobAssetStore _store;

        private void Start()
        {
            _store = new BlobAssetStore();
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
            var duckEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(duckPrefab, settings);

            for (var i = 0; i < QUANTITY; i++)
            {
                var instanceDuck = _entityManager.Instantiate(duckEntity);
                var xPos = Random.Range(-200, 200);
                var yPos = Random.Range(50, 200);
                var zPos = Random.Range(-200, 200);
                _entityManager.SetComponentData(instanceDuck,new Translation{Value = new float3(xPos,yPos,zPos)});
            }
        }
        private void OnDestroy()
        {
            _store.Dispose();
        }
    }
}
