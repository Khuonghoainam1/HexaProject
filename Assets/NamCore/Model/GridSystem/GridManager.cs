using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Các biến công khai để điều chỉnh trong Inspector
    public int rows = 10;         // Số hàng
    public int columns = 10;      // Số cột
    public float cellSpacing = 1.0f; // Khoảng cách giữa các ô
    public GameObject cellPrefab; // Prefab của ô (cell)

    // Mảng 2D để lưu trữ các ô
    private Cell[,] grid;

    void Start()
    {
        // Khởi tạo mảng grid
        grid = new Cell[rows, columns];

        // Tạo lưới bằng cách lặp qua từng hàng và cột
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Tính toán vị trí của ô dựa trên cellSpacing
                Vector3 position = new Vector3(j * cellSpacing, 0, i * cellSpacing);

                // Tạo instance của cellPrefab tại vị trí đã tính
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity);

                // Gắn cell vào GridManager để tổ chức hierarchy
                cellObj.transform.parent = this.transform;

                // Lấy component Cell từ GameObject vừa tạo
                Cell cell = cellObj.GetComponent<Cell>();

                // Khởi tạo ô với tọa độ của nó
                cell.Initialize(i, j);

                // Lưu ô vào mảng grid
                grid[i, j] = cell;
            }
        }
    }

    // Phương thức để lấy một ô theo tọa độ
    public Cell GetCell(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns)
            return grid[row, column];
        else
            return null;
    }
}