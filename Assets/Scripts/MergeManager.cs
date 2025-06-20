using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class MergeManager : MonoBehaviour
    {
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
            LayerMask girdCellMark = 1 << gridCell.gameObject.layer;
            Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, girdCellMark);

            List<GridCell> neighborGridCells = new List<GridCell>();

            foreach (Collider gridCellCollider in neighborGridCellColliders)
            {
                GridCell neighborGridCell = gridCellCollider.GetComponent<GridCell>();
                if (neighborGridCell == null || neighborGridCell == gridCell || !neighborGridCell.IsOccupied)
                    continue;

                neighborGridCells.Add(neighborGridCell);
            }

            if (neighborGridCells.Count <= 0)
            {
                Debug.Log("No neighbors for this cell");
                return;
            }

            Color gridCellTopHexagonColor = gridCell.Stack.GetHexagonColor();
            Debug.Log(gridCellTopHexagonColor);

            List<GridCell> similarNeighborGridCells = new List<GridCell>();
            foreach (GridCell neighborGridCell in neighborGridCells)
            {
                Color neighborGridCellTopHexagonColor = neighborGridCell.Stack.GetHexagonColor();
                if (gridCellTopHexagonColor == neighborGridCellTopHexagonColor)
                {
                    similarNeighborGridCells.Add(neighborGridCell);
                }
            }

            if (similarNeighborGridCells.Count <= 0)
            {
                Debug.Log("No similar neighbors for this cell");
                return;
            }

            Debug.Log($"Similar neighbors: {similarNeighborGridCells.Count}");

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

            // TODO: merge các hexagons vừa tìm được vào gridCell hiện tại.
        }
    }
}
