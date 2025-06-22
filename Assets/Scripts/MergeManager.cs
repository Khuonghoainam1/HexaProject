using System;
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
            StartCoroutine(StackPlaceCoroutine(gridCell));
        }

        IEnumerator StackPlaceCoroutine(GridCell gridCell)
        {
            yield return CheckForMerge(gridCell);

        }

        IEnumerator CheckForMerge(GridCell gridCell)
        {
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

            Debug.Log($"Similar neighbors: {similarNeighborGridCells.Count}");

            List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());


            RemoveHexagonFromStack(hexagonsToAdd, similarNeighborGridCells.ToArray());


            MoveHexagons(gridCell, hexagonsToAdd);

            yield return new WaitForSeconds(.2f + (hexagonsToAdd.Count + 1) * .01f);



             yield return CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
        }

        /*        private List<GridCell> GetNeighborGridCells(GridCell gridCell)
                {
                    LayerMask girdCellMark = 1 << gridCell.gameObject.layer;
                    List<GridCell> neighborGridCells = new List<GridCell>();

                    Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, girdCellMark);


                    foreach (Collider gridCellCollider in neighborGridCellColliders)
                    {
                        GridCell neighborGridCell = gridCellCollider.GetComponent<GridCell>();
                        if (neighborGridCell == null || neighborGridCell == gridCell || !neighborGridCell.IsOccupied)
                            continue;

                        neighborGridCells.Add(neighborGridCell);
                    }

                    return neighborGridCells;
                }*/
        private List<GridCell> GetNeighborGridCells(GridCell gridCell)
        {
            LayerMask gridCellMask = 1 << gridCell.gameObject.layer;

            List<GridCell> neighborGridCells = new List<GridCell>();

            Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2f, gridCellMask);

            foreach (Collider gridCellCollider in neighborGridCellColliders)
            {
                GridCell neighborGridCell = gridCellCollider.GetComponent<GridCell>();

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


        private void RemoveHexagonFromStack(List<Hexagon> hexagonsToAdd, GridCell[] similarNeighborGridCells)
        {
            foreach (GridCell neighborCell in similarNeighborGridCells)
            {
                HexStack stacks = neighborCell.Stack;
                foreach (Hexagon hexagon in hexagonsToAdd)
                {
                    if (stacks.Contains(hexagon))
                        stacks.Remove(hexagon);
                }
            }
        }


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


            yield return new WaitForSeconds(.2f + (simlarHexagonsCount + 1) * .01f);
        }
    }
}
