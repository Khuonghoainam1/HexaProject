/*using UnityEngine;
using System.Collections.Generic;

namespace NamCore
{
    public static class ColorDatabase
    {
        private static Dictionary<ColorID, Color> _colorDict;

        // Cache và truy xuất từ ScriptableObject
        public static Color GetColorByID(ColorID id)
        {
            if (_colorDict == null)
            {
                _colorDict = new Dictionary<ColorID, Color>();

                // Tìm SO
                var setting = Resources.Load<HexagonColorSettingSO>("HexagonColorSettingSO"); // Đặt trong folder Resources
                if (setting == null)
                {
                    Debug.LogError("Không tìm thấy HexagonColorSettingSO trong Resources!");
                    return Color.white;
                }

                foreach (var item in setting.settings)
                {
                    if (!_colorDict.ContainsKey(item.colorID))
                        _colorDict[item.colorID] = item.color;
                }
            }

            if (_colorDict.TryGetValue(id, out var color))
                return color;

            Debug.LogWarning($"Không tìm thấy màu cho ColorID: {id}");
            return Color.white;
        }
    }
}
*/