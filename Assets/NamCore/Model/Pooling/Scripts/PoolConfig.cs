// PoolConfig.cs
using UnityEngine;
using System.Collections.Generic;

namespace NamCore
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "NamCore/Pooling")]
    public class PoolConfig : ScriptableObject
    {
        public List<Pool> lstPool;
    }

    [System.Serializable]
    public class Pool
    {
        public PoolTyper poolID;
        public PoolerTarget target;
        public GameObject prefab;
        [Min(1)] public int maxSize = 1;
    }

    public enum PoolTyper
    {
        Enemy,
        Bullet,
        // Thêm các loại khác tại đây
    }

    public enum PoolerTarget
    {
        All,
        Combat,
        VFX,
        UI,
        Environment
    }
}