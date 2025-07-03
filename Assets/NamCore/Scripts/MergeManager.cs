using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NamCore
{
    public class MergeManager : MonoBehaviour
    {

        [Header("Element")]
        private List<GridCell> updateCells = new List<GridCell>();

        private void Awake()
        {
            StackController.onStackPlanced += StackPlaceCallBack;
        }

        private void OnDestroy()
        {
            StackController.onStackPlanced -= StackPlaceCallBack;
        }


        private void StackPlaceCallBack(GridCell gridCell)
        {
            StartCoroutine(StackPlaceCoroutine(gridCell));
        }

        IEnumerator StackPlaceCoroutine(GridCell gridCell)
        {
            updateCells.Add(gridCell);
            while (updateCells.Count > 0)
            {
                yield return CheckForMerge(updateCells[0]);
            }



        }

        IEnumerator CheckForMerge(GridCell gridCell)
        {
            updateCells.Remove(gridCell);
            if (!gridCell.IsOccupied) { yield break; }

            List<GridCell> neighborGridCells = GetNeighborGridCells(gridCell);
            if (neighborGridCells.Count <= 0)
            {
                Debug.Log("No neighbors for this cell");
                yield break;
            }

            Color gridCellTopHexagonColor = gridCell.Stack.GetHexagonColor();

            List<GridCell> similarNeighborGridCells = GetSimilarNeighborGridCells(gridCellTopHexagonColor, neighborGridCells.ToArray());

            if (similarNeighborGridCells.Count <= 0)
            {
                Debug.Log("No similar neighbors for this cell");
                yield break;
            }


            updateCells.AddRange(similarNeighborGridCells);

            Debug.Log($"Similar neighbors: {similarNeighborGridCells.Count}");

            List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());


            RemoveHexagonFromStack(hexagonsToAdd, similarNeighborGridCells.ToArray());


            MoveHexagons(gridCell, hexagonsToAdd);

            yield return new WaitForSeconds(.2f + (hexagonsToAdd.Count + 1) * .01f);



            yield return CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
        }

        //Tìm các ô lân cận
        private List<GridCell> GetNeighborGridCells(GridCell gridCell)
        {
            LayerMask gridCellMask = 1 << gridCell.gameObject.layer;

            List<GridCell> neighborGridCells = new List<GridCell>();

            Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2f, gridCellMask);

            foreach (Collider gridCellCollider in neighborGridCellColliders)
            {
                GridCell neighborGridCell = gridCellCollider.GetComponent<GridCell>();
                // gridcell xung quanh 
                if (neighborGridCell == null)
                    continue;

                if (!neighborGridCell.IsOccupied)
                    continue;

                if (neighborGridCell == gridCell)
                    continue;

                neighborGridCells.Add(neighborGridCell);
            }

            return neighborGridCells;
        }

        // Lọc các ô có cùng màu
        private List<GridCell> GetSimilarNeighborGridCells(Color gridCellTopHexagonColor, GridCell[] neighborGridCells)
        {
            List<GridCell> similarNeighborGridCells = new List<GridCell>();
            foreach (GridCell neighborGridCell in neighborGridCells)
            {
                Color neighborGridCellTopHexagonColor = neighborGridCell.Stack.GetHexagonColor();
                if (gridCellTopHexagonColor == neighborGridCellTopHexagonColor)
                {
                    similarNeighborGridCells.Add(neighborGridCell);
                }
            }
            return similarNeighborGridCells;
            }
            // Lấy các Hexagon cùng màu từ stack các ô giống
        private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor, GridCell[] similarNeighborGridCells)
        {
            List<Hexagon> hexagonsToAdd = new List<Hexagon>();
            foreach (GridCell neighborCell in similarNeighborGridCells)
            {                                               
                HexStack neighborStack = neighborCell.Stack;
                for (int i = neighborStack.Hexagons.Count - 1; i >= 0; i--)
                {
                    Hexagon hexagon = neighborStack.Hexagons[i];
                    if (hexagon.Color != gridCellTopHexagonColor)
                        break;

                    hexagonsToAdd.Add(hexagon);
                    hexagon.SetParent(null);
                }
            }

            return hexagonsToAdd;
        }

        //Xoá Hexagon khỏi stack cũ
        private void RemoveHexagonFromStack(List<Hexagon> hexagonsToRemove, GridCell[] similarNeighborGridCells)
        {
            foreach (GridCell neighborCell in similarNeighborGridCells)
            {
                HexStack stacks = neighborCell.Stack;
                foreach (Hexagon hexagon in hexagonsToRemove)
                {
                    if (stacks.Contains(hexagon))
                        stacks.Remove(hexagon);
                }
            }
        }

        //Thêm và di chuyển Hexagon về stack mới
        private void MoveHexagons(GridCell gridCell, List<Hexagon> hexagonsToAdd)
        {
            float initialY = gridCell.Stack.Hexagons.Count * .2f;
            for (int i = 0; i < hexagonsToAdd.Count; i++)
            {
                Hexagon hexagon = hexagonsToAdd[i];
                float targetY = initialY + i * .2f;
                Vector3 targetLocalPosition = Vector3.up * targetY;

                gridCell.Stack.Add(hexagon);
                hexagon.MoveToLocal(targetLocalPosition);

            }
        }

        // Kiểm tra stack có đủ 10 Hexagon cùng màu không
        private IEnumerator CheckForCompleteStack(GridCell gridCell, Color topColor)
        {
            if (gridCell.Stack.Hexagons.Count < 10)
                yield break;

            List<Hexagon> similarHexagons = new List<Hexagon>();
            for (int i = gridCell.Stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = gridCell.Stack.Hexagons[i];
                if (hexagon.Color != topColor)
                {
                    break;
                }
                similarHexagons.Add(hexagon);


            }

            int simlarHexagonsCount = similarHexagons.Count;

            if (similarHexagons.Count < 10)
                yield break;

            float delay = 0;
            while (similarHexagons.Count > 0)
            {

                similarHexagons[0].SetParent(null);
                similarHexagons[0].Vanish(delay);
                // DestroyImmediate(similarHexagons[0].gameObject);
                delay += .01f;

                gridCell.Stack.Remove(similarHexagons[0]);
                similarHexagons.RemoveAt(0);

            }

            updateCells.Add(gridCell);
            yield return new WaitForSeconds(.2f + (simlarHexagonsCount + 1) * .01f);
        }
    }
}



/*Stack được đặt vào GridCell
   ↓
Gọi StackPlaceCallBack
   ↓
Bắt đầu StackPlaceCoroutine
   ↓
Thêm GridCell vào updateCells
   ↓
Gọi CheckForMerge (với GridCell đầu tiên trong danh sách)
   ↓
→ Nếu có các GridCell lân cận cùng màu:
   → Di chuyển hexagons cùng màu về GridCell chính
   → Xoá hexagons cũ khỏi stack cũ
   → Kiểm tra stack mới có đủ 10 hexagons cùng màu không?
     → Nếu có, gọi Vanish và tiếp tục vòng lặp
→ Nếu không có lân cận hoặc không đủ, kết thúc*/