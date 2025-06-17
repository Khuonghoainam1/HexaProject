using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class StackController : MonoBehaviour
    {
        [Header("Setting:")]
        [SerializeField] private LayerMask m_hexagonLayerMask;
        [SerializeField] private LayerMask m_gridHexagonLayerMask;
        [SerializeField] private LayerMask m_groundLayerMask;


        private HexStack m_currentHexStack;
        private Vector3 m_currentHexStackInitialPos;

        private void Update()
        {
            ManagerControl();
        }

        private void ManagerControl()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ManagerMouseDown();
            }
            else if (Input.GetMouseButton(0) && m_currentHexStack != null)
            {
                ManagerMouseDrag();
            }
            else if (Input.GetMouseButtonUp(0) && m_currentHexStack != null)
            {
                ManagerMouseUp();
            }
        }

        private void ManagerMouseDown()
        {
            RaycastHit hit;
            Physics.Raycast(GetClickRay(), out hit, 500, m_hexagonLayerMask);
            if (hit.collider == null)
            {
                Debug.Log("We have not detected any hexagon ");
                return;
            }

            m_currentHexStack = hit.collider.GetComponent<Hexagon>().HexStack;
            m_currentHexStackInitialPos = m_currentHexStack.transform.position;
        }



        private void ManagerMouseDrag()
        {
            RaycastHit hit;
            Physics.Raycast(GetClickRay(), out hit, 500, m_gridHexagonLayerMask);

            if (hit.collider == null)
            {
                DraggingAboveGround();
            }
            else
            {
                DraggingAboveGridCell(hit);
            }
        }
    
     


        private void DraggingAboveGround()
        {
            RaycastHit hit;
            Physics.Raycast(GetClickRay(), out hit, 500, m_groundLayerMask);

            if (hit.collider == null)
            {
                Debug.LogError("No ground detected, this is unusual......");
            }
            Debug.Log("Here");


            Vector3 currentStackTargetPos = hit.point.With(y: 2);

            m_currentHexStack.transform.position = Vector3.MoveTowards(
                m_currentHexStack.transform.position,
                currentStackTargetPos,
                Time.deltaTime * 30);
        }
        private void DraggingAboveGridCell(RaycastHit hit)
        {
            GridCell gridCell = hit.collider.GetComponent<GridCell>();

            if (gridCell.IsOccupied)
                DraggingAboveGround();
            else
                DraggingAboveNonOccupiedGridCell(gridCell);
        }

 
        private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
        {
            Vector3 currentStackTargetPos = gridCell.transform.parent.position.With(y: 2);

            m_currentHexStack.transform.position = Vector3.MoveTowards(
                m_currentHexStack.transform.position,
                currentStackTargetPos,
                Time.deltaTime * 30);

        }


        private void ManagerMouseUp()
        {

        }
        private Ray GetClickRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
