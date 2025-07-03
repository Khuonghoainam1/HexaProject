using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace NameCore
{
    public class SettingsPopup : PopupBase
    {
        [SerializeField] private Button m_Close;

        public void OnEnable()
        {
            m_Close.onClick.RemoveAllListeners();
            m_Close.onClick.AddListener(OnClickCloseButton);
        }
        public void OnClickCloseButton()
        {
            Close();
        }
    }
}

