using UnityEngine;
using UnityEngine.UI;
namespace NameCore
{
    public class TestMennu  : PopupBase
    {
        [SerializeField] private Button m_btnStart;
        protected override void OnEnable()
        {
            m_btnStart.onClick.RemoveAllListeners();
            m_btnStart.onClick.AddListener(() => { GameFlowManager.Instance.StartGame(); });
        }

       
    }

}
