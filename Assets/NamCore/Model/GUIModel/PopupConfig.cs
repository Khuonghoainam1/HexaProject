using UnityEngine;
using System.Collections.Generic;

namespace NamCore
{
   

    [CreateAssetMenu(fileName = "PopupConfigTable", menuName = "UI/Popup Config Table")]
    public class PopupConfigTable : ScriptableObject
    {
        [System.Serializable]
        public class PopupEntry
        {
            public PopupType popupType;
            public GameObject popupPrefab;
        }

        public List<PopupEntry> popupEntries = new List<PopupEntry>();
    }
    public enum PopupType
    {
        None,
        Settings,
        Inventory,
        Confirmation,
        // Thêm các popup khác tại đây

        MennuView,
    }

}

