using System;
using System.Collections.Generic;
using UnityEngine; // Cần cho Vector2Int nếu bạn dùng tọa độ int

namespace NamCore
{
    [Serializable]
    public class GridCellData
    {
        public Vector2Int gridCoordinates; // Tọa độ của ô lưới (ví dụ: (0,0), (1,0)...)
        public List<HexagonData> hexagonsInStack; // Danh sách các HexagonData trong Stack của ô này

        public GridCellData(Vector2Int coordinates, List<HexagonData> hexData)
        {
            gridCoordinates = coordinates;
            hexagonsInStack = hexData;
        }
    }
}