using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace NamCore
{
    public class GridTester : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Grid m_grid;

        [Header("Settings")]
        [OnValueChanged("UpdateGridPos")]
        [SerializeField] private Vector3Int m_gridPos;


        private void UpdateGridPos() => transform.position = m_grid.CellToWorld(m_gridPos);
      
    }
}
