using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NamCore
{
    public class MainMennu : MonoBehaviour
    {
        [SerializeField] private List<Button> m_allBtnMennuBar;
        [SerializeField] private Button m_btnPlayingLevel;

        [SerializeField] private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1f);
        [SerializeField] private Vector3 normalScale = Vector3.one;

        private void Start()
        {
            foreach (var btn in m_allBtnMennuBar)
            {
                btn.onClick.AddListener(() => OnButtonClicked(btn));
            }

            m_btnPlayingLevel.onClick.RemoveListener(OnPlayLevel);
            m_btnPlayingLevel.onClick.AddListener(OnPlayLevel);
        }

        private void OnButtonClicked(Button clickedButton)
        {
            foreach (var btn in m_allBtnMennuBar)
            {
                bool isSelected = (btn == clickedButton);
                btn.transform.localScale = isSelected ? selectedScale : normalScale;
            }
        }


        private void OnPlayLevel()
        {
            GameFlowManager.Instance.StartGame();
        }
    }
}
