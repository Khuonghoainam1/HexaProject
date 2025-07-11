using System.Collections.Generic;
using NamCore;
using NaughtyAttributes;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Hexagon hexagonPrefab;

    [Header("Settings")]
    [OnValueChanged("GenerateInitalHexagons")]
    [SerializeField] private ColorID[] hexagonColors;

    public List<Hexagon> Hexagons { get; private set; } = new();
    public List<ColorID> hexagonColorIDGamePlay;
    public HexStack Stack { get; private set; }

    public bool IsOccupied => Stack != null;

    private void Start()
    {
        GridGenerator.Ins.LoadGridCellForList();

        if (hexagonColorIDGamePlay.Count > 0)
        {
            GenerateInitalHexagonsForData();
        }
        if (hexagonColors.Length > 0)
        {
            GenerateInitalHexagons();
        }
    }

    public void AssignStack(HexStack stack)
    {
        hexagonColorIDGamePlay.Clear();
        Stack = stack;
        Hexagons = stack.Hexagons;
        UpdateColorIDsAndSave();
        GridGenerator.Ins.SaveGridCellForList();
    }

    private void GenerateInitalHexagons()
    {
        // Xóa các HexStack cũ (giữ lại phần tử đầu tiên nếu là base)
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        Stack = new GameObject("Initial Stack").AddComponent<HexStack>();
        Stack.transform.SetParent(transform);
        Stack.transform.localPosition = Vector3.up * 0.2f;

        for (int i = 0; i < hexagonColors.Length; i++)
        {
            Vector3 spawnPosition = Stack.transform.TransformPoint(Vector3.up * i * 0.2f);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity);
            hexagonInstance.colorID = hexagonColors[i];
            hexagonInstance.GetColorById(hexagonInstance.colorID);
            Stack.Add(hexagonInstance);
        }

        Stack.Place();
    }


    private void GenerateInitalHexagonsForData()
    {
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        Stack = new GameObject("Initial Stack").AddComponent<HexStack>();
        Stack.transform.SetParent(transform);
        Stack.transform.localPosition = Vector3.up * 0.2f;

        for (int i = 0; i < hexagonColorIDGamePlay.Count; i++)
        {
            Vector3 spawnPosition = Stack.transform.TransformPoint(Vector3.up * i * 0.2f);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity);
            hexagonInstance.colorID = hexagonColorIDGamePlay[i];
            hexagonInstance.GetColorById(hexagonInstance.colorID);
            Stack.Add(hexagonInstance);
        }

        Stack.Place();
    }
    public void UpdateColorIDsAndSave()
    {
        hexagonColorIDGamePlay.Clear();
        for (int i = 0; i < Hexagons.Count; i++)
        {
            hexagonColorIDGamePlay.Add(Hexagons[i].colorID);
        }

        GridGenerator.Ins.SaveGridCellForList();
    }

}
