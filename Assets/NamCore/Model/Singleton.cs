using UnityEngine;

namespace NamCore
{
    /// <summary>
    /// Provides an easy way to reuse to ensure that only a 
    /// single instance of a class exists throughout the entire run of the game.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
    {
        public SenceID sceneName;
        protected int numofEnterScene;

        static T m_ins;

        public static T Ins
        {
            get
            {
                return m_ins;
            }
        }

        protected virtual void Awake()
        {
            MakeSingleton(true);
        }

        protected void MakeSingleton(bool destroyOnload)
        {
            if (m_ins == null)
            {
                m_ins = this as T;
                if (!destroyOnload) return;

                var root = transform.root;

                if (root != transform)
                {
                    DontDestroyOnLoad(root);
                }
                else
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

