using UnityEngine;

public class Cell : MonoBehaviour
{
    private int row;              // Tọa độ hàng
    private int column;           // Tọa độ cột
    private Renderer cellRenderer; // Component để thay đổi giao diện
    private bool isSelected = false; // Trạng thái chọn của ô

    void Awake()
    {
        // Lấy Renderer khi ô được tạo
        cellRenderer = GetComponent<Renderer>();
    }

    // Khởi tạo ô với tọa độ
    public void Initialize(int r, int c)
    {
        row = r;
        column = c;
    }

    // Phương thức để thay đổi trạng thái của ô (ví dụ: là tường hay đường)
    public void SetState(bool isWall)
    {
        // Thay đổi màu sắc dựa trên trạng thái
        if (isWall)
            cellRenderer.material.color = Color.black; // Tường
        else
            cellRenderer.material.color = Color.white; // Đường
    }

    // Khi ô được chọn
    public void Select()
    {
        isSelected = true;
        // Thay đổi màu sắc để biểu thị ô được chọn
        cellRenderer.material.color = Color.blue;
        Debug.Log($"Toa Do : {row}, {column}");
    }

    // Khi ô được bỏ chọn
    public void Deselect()
    {
        isSelected = false;
        // Khôi phục màu sắc ban đầu (giả sử là màu trắng)
        cellRenderer.material.color = Color.white;
    }
    void OnMouseDown()
    {
       Select();
    }

    // Khi chuột được thả
    void OnMouseUp()
    {
        Deselect();
    }
    /*private int row;              // Tọa độ hàng
    private int column;           // Tọa độ cột
    private SpriteRenderer spriteRenderer; // Component để thay đổi giao diện
    private bool isSelected = false; // Trạng thái chọn của ô

    void Awake()
    {
        // Lấy SpriteRenderer khi ô được tạo
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Khởi tạo ô với tọa độ
    public void Initialize(int r, int c)
    {
        row = r;
        column = c;
    }

    // Phương thức để thay đổi trạng thái của ô (ví dụ: là tường hay đường)
    public void SetState(bool isWall)
    {
        // Thay đổi màu sắc dựa trên trạng thái
        if (isWall)
            spriteRenderer.color = Color.black; // Tường
        else
            spriteRenderer.color = Color.white; // Đường
    }

    // Khi chuột được nhấn trên ô
    void OnMouseDown()
    {
        isSelected = true;
        // Thay đổi màu sắc để biểu thị ô được chọn
        spriteRenderer.color = Color.blue;
    }

    // Khi chuột được thả
    void OnMouseUp()
    {
        isSelected = false;
        // Khôi phục màu sắc ban đầu (giả sử là màu trắng)
        spriteRenderer.color = Color.white;
    }*/
}