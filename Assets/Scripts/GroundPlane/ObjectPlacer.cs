using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Vuforia;

[Serializable]
public class PlaceableItem
{
    public string displayName;
    public GameObject prefab;
}

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] PlaceableItem[] items;
    [SerializeField] int startingIndex;

    readonly List<GameObject> placedAnchors = new List<GameObject>();
    readonly List<RaycastResult> uiHits = new List<RaycastResult>();
    int selectedIndex;
    int anchorCount;

    public event Action<int> SelectionChanged;

    public IReadOnlyList<PlaceableItem> Items => items;
    public int SelectedIndex => selectedIndex;
    public PlaceableItem SelectedItem => items[selectedIndex];
    public int PlacedCount => placedAnchors.Count;

    void Awake()
    {
        selectedIndex = Mathf.Clamp(startingIndex, 0, items.Length - 1);
    }

    public void Select(int index)
    {
        if (index < 0 || index >= items.Length || index == selectedIndex)
            return;

        selectedIndex = index;
        SelectionChanged?.Invoke(selectedIndex);
    }

    public void SelectNext()
    {
        Select((selectedIndex + 1) % items.Length);
    }

    public void Place(HitTestResult hit)
    {
        if (hit == null || IsPointerOverUI())
            return;

        anchorCount++;
        var anchor = VuforiaBehaviour.Instance.ObserverFactory.CreateAnchorBehaviour("Anchor_" + anchorCount, hit);
        if (anchor == null)
            return;

        var placed = Instantiate(SelectedItem.prefab, anchor.transform);
        placed.transform.localPosition = Vector3.zero;
        FaceCamera(placed.transform);

        placedAnchors.Add(anchor.gameObject);
    }

    public void UndoLast()
    {
        if (placedAnchors.Count == 0)
            return;

        int last = placedAnchors.Count - 1;
        Destroy(placedAnchors[last]);
        placedAnchors.RemoveAt(last);
    }

    public void ClearAll()
    {
        foreach (var anchor in placedAnchors)
        {
            if (anchor != null)
                Destroy(anchor);
        }
        placedAnchors.Clear();
    }

    void FaceCamera(Transform target)
    {
        var cam = Camera.main;
        if (cam == null)
            return;

        Vector3 toCamera = cam.transform.position - target.position;
        toCamera.y = 0f;
        if (toCamera.sqrMagnitude > 0.0001f)
            target.rotation = Quaternion.LookRotation(toCamera, Vector3.up);
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null || Pointer.current == null)
            return false;

        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue()
        };

        uiHits.Clear();
        EventSystem.current.RaycastAll(pointerData, uiHits);
        return uiHits.Count > 0;
    }
}
