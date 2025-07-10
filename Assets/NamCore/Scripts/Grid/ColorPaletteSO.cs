using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes; // Requires NaughtyAttributes

namespace NamCore
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "NamCore/Color Palette")]
    public class ColorPaletteSO : ScriptableObject
    {
        [SerializeField]
        private List<ColorMappingEntry> colorMappings = new List<ColorMappingEntry>();

        private Dictionary<ColorID, Color> _colorDict;

        private void OnEnable()
        {
            BuildColorDictionary();
        }

        [Button("Rebuild Color Dictionary")]
        private void BuildColorDictionary()
        {
            if (colorMappings == null)
            {
                _colorDict = new Dictionary<ColorID, Color>();
                return;
            }

            _colorDict = new Dictionary<ColorID, Color>();
            foreach (var entry in colorMappings)
            {
                if (!_colorDict.ContainsKey(entry.id))
                {
                    _colorDict.Add(entry.id, entry.color);
                }
                else
                {
                    Debug.LogWarning($"Duplicate ColorID '{entry.id}' found in ColorPaletteSO '{name}'. Overwriting with the last defined color.");
                    _colorDict[entry.id] = entry.color;
                }
            }
        }

        public Color GetColorByID(ColorID id)
        {
            if (_colorDict == null || (_colorDict.Count == 0 && colorMappings.Count > 0))
            {
                BuildColorDictionary();
            }

            if (_colorDict.TryGetValue(id, out var color))
            {
                return color;
            }

            Debug.LogWarning($"Color with ID: {id} not found in ColorPaletteSO '{name}'. Returning Color.white.");
            return Color.white;
        }
    }
}