using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NamCore
{
    public class StackSpawner : MonoBehaviour
    {
        [Header("Element:")]
        [SerializeField] private Transform m_stackPositionParent;
        [SerializeField] private Hexagon m_hexagonPrefab;
        [SerializeField] private HexStack m_hexagonStack;

        [Header("Setting:")]
        [NaughtyAttributes.MinMaxSlider(2, 8)]
        [SerializeField] private Vector2Int minMaxHexCount;
        [SerializeField] private Color[] colors;

        private int m_stackCounter;


        private void Awake()
        {
            Application.targetFrameRate = 60;
            StackController.onStackPlanced += StackPlacedCellBack;
        }

        private void OnDestroy()
        {
            StackController.onStackPlanced -= StackPlacedCellBack;
        }


        private void StackPlacedCellBack(GridCell gridCell)
        {
            m_stackCounter++;

            if (m_stackCounter >= 3)
            {
                m_stackCounter = 0;
                GenerateStacks();
            }
        }


        private void Start()
        {
            GenerateStacks();
        }
        private void GenerateStacks()
        {
            for (int i = 0; i < m_stackPositionParent.childCount; i++)
            {
                GenerateStacks(m_stackPositionParent.GetChild(i));
            }
        }

        private void GenerateStacks(Transform stackPositionParent)
        {
            HexStack hexStack = Instantiate(m_hexagonStack, stackPositionParent.position, Quaternion.identity, stackPositionParent);
            hexStack.name = $"Stack {stackPositionParent.GetSiblingIndex()}";

            var _levelCtrl = GameManager.Ins.levelData.configLevelData[0];
            Color stackCorlor = colors[Random.Range(0, colors.Length)];

            int amount = Random.Range(minMaxHexCount.x, minMaxHexCount.y);
            int firstColorHexagonCount = Random.Range(0, amount);
            ColorID[] colorIDArr = _levelCtrl.GetRandomColorIDs();

            for (int i = 0; i < amount; i++)
            {
                Vector3 hexagonLocalPos = Vector3.up * i * .2f;
                Vector3 spawnerPosistion = hexStack.transform.TransformPoint(hexagonLocalPos);


                Hexagon hexagonIntance = Instantiate(m_hexagonPrefab, spawnerPosistion, Quaternion.identity, hexStack.transform);
                hexagonIntance.colorID = i < firstColorHexagonCount ? colorIDArr[0] : colorIDArr[1];
                hexagonIntance.GetColor();


                hexagonIntance.Configure(hexStack);
                hexStack.Add(hexagonIntance);
            }
        }

    }
}
