using UnityEngine;

namespace NameCore
{

    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}

