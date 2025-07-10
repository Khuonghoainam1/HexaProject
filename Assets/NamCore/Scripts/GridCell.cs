using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace NamCore
{
    public class GridCell : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Hexagon hexagonPrefab;

        [Header("Settings")]
        // Loại bỏ [OnValueChanged("GenerateInitalHexagons")] ở đây nếu bạn muốn GridManager kiểm soát hoàn toàn việc tạo lưới.
        // Nếu bạn vẫn muốn có khả năng sinh Hexagon ban đầu trực tiếp từ GridCell trong Editor,
        // bạn có thể giữ nó, nhưng cần cẩn thận để không xung đột với GridManager.
        // Hiện tại, tôi sẽ giữ nó nhưng lưu ý rằng nó có thể bị thay thế bởi logic LoadFromData.
        [OnValueChanged("GenerateInitialHexagons")] // Giữ lại cho mục đích thiết kế trong Editor
        [SerializeField] private ColorID[] initialHexagonColors; // Đổi tên thành initialHexagonColors cho rõ ràng hơn

        // Tham chiếu đến ColorPaletteSO để GridCell biết cách lấy màu thực tế từ ID
        // GridCell không nên giữ ColorPaletteSO nếu GridManager đã quản lý.
        // Thay vào đó, nó nên nhận màu từ GridManager hoặc HexagonData.
        // Tạm thời, tôi sẽ giả định Hexagon đã có Color.

        public HexStack Stack { get; private set; }

        public bool IsOccupied => Stack != null;

        private void Start()
        {
            // GridManager sẽ chịu trách nhiệm tạo hoặc tải Hexagons.
            // Phương thức GenerateInitialHexagons ở đây chủ yếu là để setup ban đầu trong Editor.
            // Nếu bạn muốn nó sinh ra khi game bắt đầu mà không có dữ liệu tải, thì giữ lại.
            if (Application.isPlaying && initialHexagonColors.Length > 0 && Stack == null)
            {
                // Chỉ gọi khi game chạy và chưa có Stack (chưa được GridManager tải)
                GenerateInitialHexagons();
            }
        }

        public void AssignStack(HexStack stack)
        {
            Stack = stack;
        }

        /// <summary>
        /// Xóa bỏ HexStack hiện tại khỏi ô lưới.
        /// </summary>
        public void ClearStack()
        {
            if (Stack != null)
            {
                // Bạn cần thêm phương thức ClearHexagons() vào lớp HexStack của mình
                // để đảm bảo các hexagon con cũng được hủy đúng cách.
                Stack.ClearHexagons();

                if (Application.isPlaying)
                {
                    Destroy(Stack.gameObject);
                }
                else
                {
                    DestroyImmediate(Stack.gameObject);
                }
                Stack = null;
            }
        }

        // Phương thức này hiện tại chủ yếu để sinh ra trong Editor bằng OnValueChanged
        // và là một fallback nếu GridManager không tải dữ liệu.
        private void GenerateInitialHexagons()
        {
            ClearStack();

            if (initialHexagonColors == null || initialHexagonColors.Length == 0)
            {
                // Debug.Log("No initial hexagons defined for " + gameObject.name);
                return;
            }

            Stack = new GameObject("Initial Stack").AddComponent<HexStack>();
            Stack.transform.SetParent(transform);
            Stack.transform.localPosition = Vector3.up * .2f;

            for (int i = 0; i < initialHexagonColors.Length; i++)
            {
                Vector3 spawnPositon = Stack.transform.TransformPoint(Vector3.up * i * .2f);
                Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPositon, Quaternion.identity);

                // Gán ColorID. GridManager sẽ gán màu thực tế khi tải.
                hexagonInstance.colorID = initialHexagonColors[i];
                // hexagonInstance.Color = actual color from palette; // GridCell không nên tự lấy màu ở đây

                Stack.Add(hexagonInstance);
            }
        }

        /// <summary>
        /// Lưu dữ liệu của GridCell này vào một đối tượng GridCellData.
        /// </summary>
        /// <param name="gridCoords">Tọa độ của ô lưới này.</param>
        /// <returns>Đối tượng GridCellData chứa trạng thái hiện tại.</returns>
        public GridCellData SaveCellData(Vector2Int gridCoords)
        {
            List<HexagonData> hexesData = new List<HexagonData>();
            if (Stack != null)
            {
                foreach (Hexagon hex in Stack.GetHexagons()) // Bạn cần thêm GetHexagons() vào HexStack
                {
                    hexesData.Add(new HexagonData(hex.colorID));
                }
            }
            return new GridCellData(gridCoords, hexesData);
        }

        /// <summary>
        /// Tải dữ liệu vào GridCell này từ một đối tượng GridCellData.
        /// </summary>
        /// <param name="data">Dữ liệu GridCellData để tải.</param>
        /// <param name="colorPalette">ColorPaletteSO để lấy màu từ ID.</param>
        public void LoadCellData(GridCellData data, ColorPaletteSO colorPalette)
        {
            ClearStack(); // Xóa stack cũ trước khi tải cái mới

            if (data.hexagonsInStack == null || data.hexagonsInStack.Count == 0)
            {
                // Debug.Log($"No hexagons to load for cell at {data.gridCoordinates}");
                return;
            }

            Stack = new GameObject("Loaded Stack").AddComponent<HexStack>();
            Stack.transform.SetParent(transform);
            Stack.transform.localPosition = Vector3.up * .2f;

            for (int i = 0; i < data.hexagonsInStack.Count; i++)
            {
                HexagonData hexData = data.hexagonsInStack[i];
                Vector3 spawnPosition = Stack.transform.TransformPoint(Vector3.up * i * .2f);

                Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity);
                hexagonInstance.colorID = hexData.colorID;

                // Lấy màu thực tế từ ColorPaletteSO
               // hexagonInstance.Color = colorPalette.GetColorByID(hexData.colorID);

                Stack.Add(hexagonInstance);
            }
            // Debug.Log($"Loaded {data.hexagonsInStack.Count} hexagons for cell at {data.gridCoordinates}");
        }
    }
}