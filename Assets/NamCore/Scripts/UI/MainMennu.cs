using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NamCore
{
    public class MainMennu : MonoBehaviour
    {
        [SerializeField] private List<Button> allButtons;

        [SerializeField] private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1f);
        [SerializeField] private Vector3 normalScale = Vector3.one;

        private void Start()
        {
            foreach (var btn in allButtons)
            {
                btn.onClick.AddListener(() => OnButtonClicked(btn));
            }
        }

        private void OnButtonClicked(Button clickedButton)
        {
            foreach (var btn in allButtons)
            {
                bool isSelected = (btn == clickedButton);
                btn.transform.localScale = isSelected ? selectedScale : normalScale;
            }
        }
    }
}
