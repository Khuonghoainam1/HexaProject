using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NamCore
{
    /// <summary>
    /// Quản lý việc lưu, tải dữ liệu game dạng JSON.
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        public GameData Data { get; private set; } = new();

        private string m_dataFilePath;

        private const string SaveKey = "GAME_DATA";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                m_dataFilePath = Path.Combine(Application.persistentDataPath, "GameData.json");
                LoadData();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Lưu dữ liệu GameData dưới dạng JSON vào file.
        /// </summary>
        public void SaveData()
        {
            if (Data == null)
            {
                Debug.LogWarning("[DataManager] SaveData: Data is null, không thể lưu.");
                return;
            }

            string json = JsonUtility.ToJson(Data, true); // true để format đẹp dễ đọc
            File.WriteAllText(m_dataFilePath, json);
            Debug.Log($"[DataManager] SaveData: Data saved at path: {m_dataFilePath}");
        }

        /// <summary>
        /// Tải dữ liệu GameData từ file JSON nếu tồn tại.
        /// </summary>
        public void LoadData()
        {
            if (!File.Exists(m_dataFilePath))
            {
                Debug.LogWarning($"[DataManager] LoadData: File không tồn tại tại đường dẫn: {m_dataFilePath}");
                return;
            }

            string json = File.ReadAllText(m_dataFilePath);
            JsonUtility.FromJsonOverwrite(json, Data);

            DebugOpenDataFilePath(m_dataFilePath);

            Debug.Log($"[DataManager] LoadData: Data loaded from path: {m_dataFilePath}");
        }

        /// <summary>
        /// Xóa dữ liệu lưu trong PlayerPrefs và xóa file JSON.
        /// </summary>
        public void ResetData()
        {
            PlayerPrefs.DeleteKey(SaveKey);

            if (File.Exists(m_dataFilePath))
            {
                File.Delete(m_dataFilePath);
                Debug.Log($"[DataManager] ResetData: Đã xóa file dữ liệu tại {m_dataFilePath}");
            }

            Data = new GameData();
        }

        /// <summary>
        /// Hiển thị đường dẫn file dữ liệu và hỗ trợ mở file hoặc thư mục chứa file trong Editor.
        /// </summary>
        private void DebugOpenDataFilePath(string path)
        {
#if UNITY_EDITOR
            // Tạo đường dẫn asset dựa trên đường dẫn tuyệt đối
            string assetPath = "Assets" + path.Substring(Application.dataPath.Length).Replace("\\", "/");
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset != null)
            {
                Debug.Log($"[DataManager] Click để mở file dữ liệu: {assetPath}");
                Debug.developerConsoleVisible = true;
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
            else
            {
                Debug.Log($"[DataManager] Đường dẫn file dữ liệu (bấm để mở thư mục chứa): {path}");
                EditorUtility.RevealInFinder(path);
            }
#endif
        }
    }
}
