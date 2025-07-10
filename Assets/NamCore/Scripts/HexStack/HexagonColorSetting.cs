using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{

    [CreateAssetMenu(fileName = "HexagonColorSettingSO", menuName = "HexaStacks/HexagonColorSettingSO", order = 0)]
    public class HexagonColorSettingSO: ScriptableObject
    {


        public List<HexagonColorSetting> settings;
    }


    [Serializable]
    public class HexagonColorSetting 
    {
        public ColorID colorID;
        public Color color;
    }

}
