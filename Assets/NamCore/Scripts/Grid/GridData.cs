using System;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    [Serializable]
    public class GridData
    {
        public int gridWidth;
        public int gridHeight;
        public List<GridCellData> allGridCellsData;
        public int amout;
        public List<HexagonData> hexagonDatas = new();
        public GridData()
        {
            allGridCellsData = new List<GridCellData>();
        }
    }
}