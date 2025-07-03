using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;

namespace NamCore
{
    public class GridGenerator : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Grid m_gird;
        [SerializeField] private GameObject m_hexagon;

        [Header("Setting")]
        [OnValueChanged("GeneratGird")]
        [SerializeField] private int m_girdSize;


        private void GeneratGird()
        {
            transform.Clear();
            for(int x = -m_girdSize; x<= m_girdSize; x++)
            {
                for(int y = -m_girdSize; y<= m_girdSize; y++)
                {
                   
                    Vector3 spawnPos = m_gird.CellToWorld(new Vector3Int(x, y, 0));
                    if(spawnPos.magnitude > m_gird.CellToWorld(new Vector3Int(1, 0 , 0)).magnitude * m_girdSize){
                        continue;
                    }
                    GameObject gridHexInstance = (GameObject)PrefabUtility.InstantiatePrefab(m_hexagon);
                    gridHexInstance.transform.position = spawnPos;
                    gridHexInstance.transform.rotation = Quaternion.identity;
                    gridHexInstance.transform.SetParent(transform);

                   // Instantiate(m_hexagon, spawnPos, Quaternion.identity, transform );
                }
            }
        }
    }
}
#endif