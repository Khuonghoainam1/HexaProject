using UnityEngine;
using System;

namespace NamCore
{
    [Serializable]
    public class HexagonColorSetting
    {
        [HideInInspector]
        public ColorPaletteSO colorPalette;

        public ColorID colorID;

        [HideInInspector]
        public Color actualColor;
    }
}