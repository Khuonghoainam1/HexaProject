using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace NamCore
{
    public class GridGenerator : Singleton<GridGenerator>
    {
        [Header("Elements")]
        [SerializeField] private Grid m_grid;
        [SerializeField] private GameObject m_hexagonPrefab;

        [Header("Setting")]
        [SerializeField] private int m_gridSize;
        [SerializeField] private List<GameObject> m_cells = new List<GameObject>();

        public int level;

        public List<GameObject> Cells => m_cells;
        public Grid Grid => m_grid;
        public GameObject HexagonPrefab => m_hexagonPrefab;
        public int GridSize => m_gridSize;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) SaveGridCellForList();
            if (Input.GetKeyDown(KeyCode.S)) LoadGridCellForList();
        }

        public void LoadGridCellForList()
        {
            var data = DataManager.Instance.Data;

            if (level >= data.levelGamePlayData.Count)
            {
                Debug.LogWarning($"⚠ Không có dữ liệu cho level {level}");
                return;
            }

            List<ColorGridCellData> cellDataList = data.levelGamePlayData[level].cellData;

            for (int i = 0; i < m_cells.Count; i++)
            {
                if (i >= cellDataList.Count) continue;

                GridCell gridCell = m_cells[i].GetComponent<GridCell>();
                gridCell.hexagonColorIDGamePlay = cellDataList[i].color;
            }

            Debug.Log($"✅ Đã load dữ liệu GridCell cho level {level}");
        }

        public void SaveGridCellForList()
        {
            var data = DataManager.Instance.Data;

            while (data.levelGamePlayData.Count <= level)
                data.levelGamePlayData.Add(new LevelGamePlayData());

            data.levelGamePlayData[level].cellData.Clear();

            for (int i = 0; i < m_cells.Count; i++)
            {
                GridCell gridCell = m_cells[i].GetComponent<GridCell>();
                ColorGridCellData cellData = new ColorGridCellData
                {
                    color = gridCell.hexagonColorIDGamePlay
                };

                data.levelGamePlayData[level].cellData.Add(cellData);
            }

            Debug.Log($"✅ Đã lưu dữ liệu cell cho level {level}");
            DataManager.Instance.SaveData();
        }

        public void ClearExistingCells()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            m_cells.Clear();
        }
    }
}
