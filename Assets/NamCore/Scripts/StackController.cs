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


        private HexStack m_currentStack;
        private Vector3 m_currentHexStackInitialPos;


        [Header("Data")]
        private GridCell m_targetCell;


        [Header("Action")]

        public static Action<GridCell> onStackPlanced;




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
            else if (Input.GetMouseButton(0) && m_currentStack != null)
            {
                ManagerMouseDrag();
            }
            else if (Input.GetMouseButtonUp(0) && m_currentStack != null)
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

            m_currentStack = hit.collider.GetComponent<Hexagon>().HexStack;

         
            m_currentHexStackInitialPos = m_currentStack.transform.position;
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


            Vector3 currentStackTargetPos = hit.point.With(y: 2);

            m_currentStack.transform.position = Vector3.MoveTowards(
                m_currentStack.transform.position,
                currentStackTargetPos,
                Time.deltaTime * 30);

            m_targetCell = null;

        }
        private void DraggingAboveGridCell(RaycastHit hit)
        {
            GridCell gridCell = hit.collider.GetComponent<GridCell>();

            if (gridCell.IsOccupied)
              {  DraggingAboveGround();
            }
            else
                DraggingAboveNonOccupiedGridCell(gridCell);
        }

 
        private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
        {
            Vector3 currentStackTargetPos = gridCell.transform.position.With(y: 2);

            m_currentStack.transform.position = Vector3.MoveTowards(
                m_currentStack.transform.position,
                currentStackTargetPos,
                Time.deltaTime * 30);

           m_targetCell = gridCell;
            //gridCell.renderer.material = gridCell.onMaterial;

        }


        private void ManagerMouseUp()
        {
            if(m_targetCell == null)
            {
                m_currentStack.transform.position = m_currentHexStackInitialPos;
                m_currentStack = null;
                return;
            }

            m_currentStack.transform.position = m_targetCell.transform.position.With(y: .2f);
            m_currentStack.transform.SetParent(m_targetCell.transform);
            m_currentStack.Place();
            m_targetCell.AssignStack(m_currentStack);
            onStackPlanced?.Invoke(m_targetCell);


            m_targetCell = null;


            m_currentStack = null;
        }
        private Ray GetClickRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
