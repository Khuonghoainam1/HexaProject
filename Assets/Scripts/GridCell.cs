using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{

    public class GridCell : MonoBehaviour
    {
        private HexStack m_stack;


      public bool IsOccupied { 
            
            get => m_stack != null;


            private set { } }
    }
}
