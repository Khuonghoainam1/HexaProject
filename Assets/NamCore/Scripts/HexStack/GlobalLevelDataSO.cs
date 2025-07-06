using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NamCore
{

    [CreateAssetMenu(fileName = "LevelConfig", menuName = "HexaStack/ColorData", order = 1)]

    public class GlobalLevelDataSO : ScriptableObject
    {
        public List<LevelDataConfig> configLevelData;
    }
    [Serializable]
    public class LevelDataConfig
    {
        public int level;
        public List<ColorDataSO> colorData;

        private Dictionary<ColorID, Color> _colorDict;

        // Trả về màu theo ColorID
        public Color GetColorByID(ColorID id)
        {
            if (_colorDict == null)
            {
                _colorDict = new Dictionary<ColorID, Color>();
                foreach (var data in colorData)
                {
                    if (!_colorDict.ContainsKey(data.id))
                        _colorDict[data.id] = data.color;
                }
            }

            if (_colorDict.TryGetValue(id, out var color))
                return color;

            Debug.LogWarning($"Không tìm thấy màu cho ID: {id}");
            return Color.white;
        }
        public ColorID[] GetRandomColorIDs()
        {
            if (colorData == null || colorData.Count < 2)
            {
                Debug.LogError("Không đủ ColorID để chọn ngẫu nhiên 2 cái.");
                return null;
            }

            // Xáo trộn danh sách và chọn 2 ID đầu tiên
            var shuffled = colorData.OrderBy(x => UnityEngine.Random.value).ToList();
            return new ColorID[] { shuffled[0].id, shuffled[1].id };
        }
        // Trả về 2 màu ngẫu nhiên khác nhau từ danh sách colorData
        /*  public Color[] GetRandomColor()
          {
              if (colorData == null || colorData.Count < 2)
              {
                  Debug.LogError("Không đủ màu trong colorData để chọn ngẫu nhiên 2 màu.");
                  return null;
              }

              var shuffled = colorData.OrderBy(x => UnityEngine.Random.value).ToList();
              return new Color[] { shuffled[0].color, shuffled[1].color };
          }*/
    }
    [Serializable]
    public class ColorDataSO 
    {
        public ColorID id;
        public Color color;
       
    }
}
