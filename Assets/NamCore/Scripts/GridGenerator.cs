using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;

namespace NamCore
{
    public class GridGenerator : Singleton<GridGenerator>
    {
        [Header("Elements")]
        [SerializeField] private Grid m_grid; // Đối tượng Grid component
        [SerializeField] private GameObject m_hexagonPrefab; // Prefab của ô lục giác

        [Header("Setting")]
        [OnValueChanged("GenerateGrid")] // Gọi GenerateGrid() khi giá trị này thay đổi
        [SerializeField] private int m_gridSize; // Kích thước lưới (bán kính)
        [SerializeField] private List<GameObject> m_cells = new List<GameObject>(); // Danh sách các ô lưới đã tạo

        public int level;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                SaveGridCellForList();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                LoadGridCellForList();

            }
        }
        // Phương thức chính để tạo hoặc cập nhật lưới
        private void GenerateGrid()
        {
            ClearExistingCells(); // Xóa tất cả các ô lưới hiện có

            for (int x = -m_gridSize; x <= m_gridSize; x++)
            {
                for (int y = -m_gridSize; y <= m_gridSize; y++)
                {
                    Vector3 cellWorldPos = m_grid.CellToWorld(new Vector3Int(x, y, 0));

                    // Bỏ qua các ô nằm ngoài phạm vi hình tròn mong muốn
                    if (cellWorldPos.magnitude > m_grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * m_gridSize)
                    {
                        continue;
                    }

                    // Tạo và cấu hình ô lục giác mới
                    GameObject newHex = (GameObject)PrefabUtility.InstantiatePrefab(m_hexagonPrefab);
                    newHex.transform.SetPositionAndRotation(cellWorldPos, Quaternion.identity);
                    newHex.transform.SetParent(transform);

                    m_cells.Add(newHex); // Thêm vào danh sách quản lý
                }
            }
        }

        // Phương thức trợ giúp để xóa các ô lưới cũ
        private void ClearExistingCells()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject); // Xóa ngay lập tức trong Editor
            }
            m_cells.Clear(); // Dọn dẹp danh sách tham chiếu
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
                if (i >= cellDataList.Count)
                {
                    Debug.LogWarning($"⚠ Không có dữ liệu cho cell {i} trong level {level}, bỏ qua");
                    continue;
                }

                GridCell gridCell = m_cells[i].GetComponent<GridCell>();
                gridCell.hexagonColorIDGamePlay = cellDataList[i].color;
            }

            Debug.Log($"✅ Đã load dữ liệu GridCell cho level {level}");
        }


        public void SaveGridCellForList()
        {
            var data = DataManager.Instance.Data;

            // Đảm bảo danh sách đủ cấp độ level
            while (data.levelGamePlayData.Count <= level)
            {
                data.levelGamePlayData.Add(new LevelGamePlayData());
            }

            // Xóa dữ liệu cũ của level
            data.levelGamePlayData[level].cellData.Clear();

            // Ghi dữ liệu mới
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


       
    }
}
#endif