using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Cần cho File.Exists, File.ReadAllText, File.WriteAllText
using System.Linq; // Cần cho ToDictionary()

namespace NamCore
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; } // Singleton pattern

        [Header("Grid Setup")]
        [SerializeField] private GridCell gridCellPrefab; // Prefab của GridCell
        [SerializeField] private int gridWidth = 5;
        [SerializeField] private int gridHeight = 5;
        [SerializeField] private float cellSize = 1f; // Kích thước mỗi ô lưới
        [SerializeField] private Vector3 gridOrigin = Vector3.zero; // Điểm gốc của lưới

        [Header("Data Management")]
        [SerializeField] private ColorPaletteSO colorPalette; // Tham chiếu đến ColorPaletteSO
        private string saveFilePath; // Đường dẫn file lưu game

        private Dictionary<Vector2Int, GridCell> _gridCells; // Dictionary để truy cập GridCell nhanh chóng

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Để GridManager tồn tại giữa các scene
            }

            saveFilePath = Path.Combine(Application.persistentDataPath, "gridSaveData.json");
            _gridCells = new Dictionary<Vector2Int, GridCell>();

            // Tải lưới khi khởi động hoặc tạo mới nếu không có file save
            LoadGrid();
        }

        // --- Grid Generation ---
        [ContextMenu("Generate New Grid")] // Thêm option vào Inspector context menu
        public void GenerateNewGrid()
        {
            ClearGrid(); // Xóa lưới cũ trước khi tạo mới

            // Tạo lưới các GridCell
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Vector2Int coords = new Vector2Int(x, y);
                    Vector3 spawnPos = GetCellWorldPosition(coords);

                    GridCell newCell = Instantiate(gridCellPrefab, spawnPos, Quaternion.identity, transform);
                    newCell.name = $"GridCell_{x}_{y}";
                    _gridCells.Add(coords, newCell);

                    // GridCell sẽ tự gọi GenerateInitialHexagons nếu có initialHexagonColors trong Prefab
                    // Hoặc bạn có thể thêm logic tạo Hexagon ngẫu nhiên ở đây
                }
            }
            Debug.Log($"Generated a new grid of {gridWidth}x{gridHeight} cells.");
        }

        private Vector3 GetCellWorldPosition(Vector2Int coords)
        {
            // Logic tính toán vị trí thế giới cho mỗi ô lưới
            // Có thể điều chỉnh cho các loại lưới khác nhau (vuông, lục giác...)
            float xOffset = coords.x * cellSize;
            float yOffset = coords.y * cellSize;
            return gridOrigin + new Vector3(xOffset, 0, yOffset);
        }

        /// <summary>
        /// Xóa tất cả các ô lưới hiện có.
        /// </summary>
        [ContextMenu("Clear Grid")]
        public void ClearGrid()
        {
            if (_gridCells != null)
            {
                foreach (var kvp in _gridCells)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.ClearStack(); // Đảm bảo stack cũng bị hủy
                        if (Application.isPlaying)
                        {
                            Destroy(kvp.Value.gameObject);
                        }
                        else
                        {
                            DestroyImmediate(kvp.Value.gameObject);
                        }
                    }
                }
                _gridCells.Clear();
                Debug.Log("Grid cleared.");
            }

            // Xóa mọi child còn sót lại mà không phải của GridManager
            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }


        // --- Save / Load Data ---
        [ContextMenu("Save Grid Data")]
        public void SaveGrid()
        {
            GridData saveData = new GridData
            {
                gridWidth = this.gridWidth,
                gridHeight = this.gridHeight,
                allGridCellsData = new List<GridCellData>()
            };

            // Duyệt qua tất cả GridCell và lưu dữ liệu của chúng
            foreach (var kvp in _gridCells)
            {
                GridCell cell = kvp.Value;
                Vector2Int coords = kvp.Key;
                saveData.allGridCellsData.Add(cell.SaveCellData(coords));
            }

            string json = JsonUtility.ToJson(saveData, true); // true để dễ đọc
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Grid data saved to: {saveFilePath}");
        }

        [ContextMenu("Load Grid Data")]
        public void LoadGrid()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                GridData loadedData = JsonUtility.FromJson<GridData>(json);

                ClearGrid(); // Xóa lưới hiện tại trước khi tải cái mới

                this.gridWidth = loadedData.gridWidth;
                this.gridHeight = loadedData.gridHeight;

                // Tái tạo lưới từ dữ liệu tải
                foreach (GridCellData cellData in loadedData.allGridCellsData)
                {
                    Vector3 spawnPos = GetCellWorldPosition(cellData.gridCoordinates);
                    GridCell newCell = Instantiate(gridCellPrefab, spawnPos, Quaternion.identity, transform);
                    newCell.name = $"GridCell_{cellData.gridCoordinates.x}_{cellData.gridCoordinates.y}";
                    _gridCells.Add(cellData.gridCoordinates, newCell);

                    // Tải dữ liệu cụ thể cho từng ô GridCell
                    newCell.LoadCellData(cellData, colorPalette); // Truyền ColorPaletteSO vào đây
                }
                Debug.Log($"Grid data loaded from: {saveFilePath}");
            }
            else
            {
                Debug.LogWarning("No save file found. Generating a new default grid.");
                GenerateNewGrid(); // Tạo lưới mới nếu không có file save
                SaveGrid(); // Lưu lưới mặc định lần đầu
            }
        }

        // Phương thức truy cập để lấy GridCell tại một tọa độ
        public GridCell GetCell(Vector2Int coords)
        {
            _gridCells.TryGetValue(coords, out GridCell cell);
            return cell;
        }

        // --- Editor Visualisation (Optional, for debugging) ---
        // private void OnDrawGizmos()
        // {
        //     if (_gridCells == null) return;

        //     Gizmos.color = Color.blue;
        //     foreach (var kvp in _gridCells)
        //     {
        //         Vector3 center = kvp.Value.transform.position;
        //         // Draw a cube to represent the cell's space
        //         Gizmos.DrawWireCube(center, Vector3.one * cellSize * 0.9f);
        //         // Optionally draw coordinates
        //         // Handles.Label(center + Vector3.up * 0.5f, kvp.Key.ToString());
        //     }
        // }
    }
}