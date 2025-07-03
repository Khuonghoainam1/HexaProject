using UnityEngine;

namespace NamCore
{

    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}

