using System;
using UnityEngine;

namespace NamCore
{
    [Serializable]
    public class HexagonData
    {
        public ColorID colorID;

        public HexagonData(ColorID id) // This is a constructor
        {
            colorID = id;
        }
    }
}