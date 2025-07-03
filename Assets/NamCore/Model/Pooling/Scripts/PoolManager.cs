// PoolManager.cs
using UnityEngine;
using System.Collections.Generic;

namespace NamCore
{
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public class PoolSettings
        {
            public bool dontDestroyOnLoad = false;
            public bool autoExpand = true;
        }

        [SerializeField] private PoolConfig poolConfig;
        [SerializeField] private PoolSettings globalSettings;

        private Dictionary<PoolerTarget, Dictionary<PoolTyper, Queue<GameObject>>> poolDictionary;
        private Dictionary<PoolerTarget, Transform> targetContainers;
        private Dictionary<GameObject, PoolableReference> activeObjects;

        public static PoolManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializePools();

            if (globalSettings.dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        private void InitializePools()
        {
            poolDictionary = new Dictionary<PoolerTarget, Dictionary<PoolTyper, Queue<GameObject>>>();
            targetContainers = new Dictionary<PoolerTarget, Transform>();
            activeObjects = new Dictionary<GameObject, PoolableReference>();

            foreach (Pool pool in poolConfig.lstPool)
            {
                // Tạo container cho target nếu chưa tồn tại
                if (!targetContainers.ContainsKey(pool.target))
                {
                    CreateTargetContainer(pool.target);
                }

                // Tạo container cho pool type
                var typeContainer = new GameObject($"{pool.poolID}_Pool").transform;
                typeContainer.SetParent(targetContainers[pool.target]);

                // Khởi tạo queue
                if (!poolDictionary.ContainsKey(pool.target))
                {
                    poolDictionary.Add(pool.target, new Dictionary<PoolTyper, Queue<GameObject>>());
                }

                Queue<GameObject> objectQueue = new Queue<GameObject>();

                for (int i = 0; i < pool.maxSize; i++)
                {
                    GameObject obj = CreatePooledObject(pool, typeContainer);
                    objectQueue.Enqueue(obj);
                }

                poolDictionary[pool.target].Add(pool.poolID, objectQueue);
            }
        }

        private void CreateTargetContainer(PoolerTarget target)
        {
            var container = new GameObject($"{target}_Container").transform;
            container.SetParent(transform);

            if (globalSettings.dontDestroyOnLoad)
                DontDestroyOnLoad(container.gameObject);

            targetContainers.Add(target, container);
        }

        private GameObject CreatePooledObject(Pool pool, Transform parent)
        {
            GameObject obj = Instantiate(pool.prefab, parent);
            obj.SetActive(false);

            var reference = obj.AddComponent<PoolableReference>();
            reference.target = pool.target;
            reference.poolType = pool.poolID;

            return obj;
        }

        public GameObject Spawn(
            PoolerTarget target,
            PoolTyper poolType,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null){
            if (!poolDictionary.ContainsKey(target) || !poolDictionary[target].ContainsKey(poolType))
            {
                Debug.LogError($"Pool {target}/{poolType} not found!");
                return null;
            }

            Queue<GameObject> poolQueue = poolDictionary[target][poolType];

            // Auto expand nếu enabled
            if (poolQueue.Count == 0 && globalSettings.autoExpand)
            {
                ExpandPool(target, poolType);
            }

            if (poolQueue.Count == 0)
            {
                Debug.LogWarning($"Pool {target}/{poolType} is empty!");
                return null;
            }

            GameObject obj = poolQueue.Dequeue();
            SetupSpawnedObject(obj, position, rotation, parent);

            // Tracking
            activeObjects.Add(obj, obj.GetComponent<PoolableReference>());

            return obj;
        }

        private void ExpandPool(PoolerTarget target, PoolTyper poolType)
        {
            Pool pool = poolConfig.lstPool.Find(p =>
                p.target == target &&
                p.poolID == poolType
            );

            if (pool != null)
            {
                Transform parent = targetContainers[target].Find($"{poolType}_Pool");
                GameObject newObj = CreatePooledObject(pool, parent);
                poolDictionary[target][poolType].Enqueue(newObj);
            }
        }

        private void SetupSpawnedObject(GameObject obj, Vector3 pos, Quaternion rot, Transform parent)
        {
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.transform.SetParent(parent);
            obj.SetActive(true);

            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnSpawn();

            if (globalSettings.dontDestroyOnLoad)
                DontDestroyOnLoad(obj);
        }

        public void Despawn(GameObject obj)
        {
            if (!activeObjects.ContainsKey(obj))
            {
                Debug.LogWarning("Object not from pool: " + obj.name);
                return;
            }

            PoolableReference reference = activeObjects[obj];
            ResetObject(obj, reference);

            // Return to pool
            poolDictionary[reference.target][reference.poolType].Enqueue(obj);
            activeObjects.Remove(obj);
        }

        private void ResetObject(GameObject obj, PoolableReference reference)
        {
            obj.SetActive(false);
            obj.transform.SetParent(targetContainers[reference.target].Find($"{reference.poolType}_Pool"));

            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnDespawn();
        }

        // Bulk Operations
        public void ClearAll()
        {
            foreach (var kvp in activeObjects)
            {
                ResetObject(kvp.Key, kvp.Value);
                Destroy(kvp.Key);
            }
            activeObjects.Clear();
        }

        public void RollbackToPool(PoolerTarget target = PoolerTarget.All)
        {
            List<GameObject> toRemove = new List<GameObject>();

            foreach (var kvp in activeObjects)
            {
                if (target == PoolerTarget.All || kvp.Value.target == target)
                {
                    ResetObject(kvp.Key, kvp.Value);
                    poolDictionary[kvp.Value.target][kvp.Value.poolType].Enqueue(kvp.Key);
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var obj in toRemove)
            {
                activeObjects.Remove(obj);
            }
        }
    }

    public class PoolableReference : MonoBehaviour
    {
        public PoolerTarget target;
        public PoolTyper poolType;
    }

}