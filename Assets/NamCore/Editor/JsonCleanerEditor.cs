#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class JsonCleanerEditor : EditorWindow
{
    [MenuItem("Tools/Clean Saved JSON Files")]
    public static void CleanJsonFiles()
    {
        string path = Application.persistentDataPath;

        // Lấy tất cả file .json trong thư mục
        string[] jsonFiles = Directory.GetFiles(path, "*.json");

        int deleteCount = 0;
        foreach (string file in jsonFiles)
        {
            File.Delete(file);
            deleteCount++;
        }

        EditorUtility.DisplayDialog("Xoá JSON", $"Đã xoá {deleteCount} file JSON tại:\n{path}", "OK");
        Debug.Log($"Đã xoá {deleteCount} file JSON từ {path}");
    }
}
#endif
