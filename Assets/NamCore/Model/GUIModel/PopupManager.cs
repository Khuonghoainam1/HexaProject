using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
namespace NameCore
{

    public class PopupManager : MonoBehaviour
    {
        public static PopupManager Instance;

        [Header("Config Table")]
        public PopupConfigTable popupConfigTable;

        private Dictionary<PopupType, PopupBase> pool = new Dictionary<PopupType, PopupBase>();
        private Dictionary<PopupType, GameObject> prefabMap = new Dictionary<PopupType, GameObject>();

        private void Awake()
        {
            if (Instance == null) Instance = this;

            // Load tất cả các popup vào prefabMap
            foreach (var entry in popupConfigTable.popupEntries)
            {
                if (!prefabMap.ContainsKey(entry.popupType))
                {
                    prefabMap.Add(entry.popupType, entry.popupPrefab);
                }
            }
        }

        public void ShowPopup(PopupType type, System.Action onClose = null)
        {
            if (!pool.ContainsKey(type))
            {
                if (!prefabMap.TryGetValue(type, out var prefab))
                {
                    Debug.LogWarning($"Không tìm thấy prefab cho popup: {type}");
                    return;
                }

                var popupGO = Instantiate(prefab, transform);
                var popup = popupGO.GetComponent<PopupBase>();
                popupGO.SetActive(false);
                pool[type] = popup;
            }

            var popupInstance = pool[type];
            popupInstance.onCloseCallback = onClose;
            popupInstance.gameObject.SetActive(true);
        }

        public void ClosePopup(PopupType type)
        {
            if (pool.TryGetValue(type, out var popup))
            {
                popup.Close();
            }
        }
    }

}

