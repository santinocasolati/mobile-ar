using UnityEngine;
using UnityEngine.UI;

public class PlacementMenu : MonoBehaviour
{
    [SerializeField] ObjectPlacer placer;
    [SerializeField] Button[] itemButtons;
    [SerializeField] Button undoButton;
    [SerializeField] Button clearButton;
    [SerializeField] Text selectedLabel;
    [SerializeField] Color normalColor = new Color(1f, 1f, 1f, 0.85f);
    [SerializeField] Color selectedColor = new Color(1f, 0.6f, 0.15f, 1f);

    void Start()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i;
            itemButtons[i].onClick.AddListener(() => placer.Select(index));

            var label = itemButtons[i].GetComponentInChildren<Text>();
            if (label != null && index < placer.Items.Count)
                label.text = placer.Items[index].displayName;
        }

        undoButton.onClick.AddListener(placer.UndoLast);
        clearButton.onClick.AddListener(placer.ClearAll);

        placer.SelectionChanged += Refresh;
        Refresh(placer.SelectedIndex);
    }

    void OnDestroy()
    {
        if (placer != null)
            placer.SelectionChanged -= Refresh;
    }

    void Update()
    {
        bool hasObjects = placer.PlacedCount > 0;
        undoButton.interactable = hasObjects;
        clearButton.interactable = hasObjects;
    }

    void Refresh(int selected)
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].image.color = i == selected ? selectedColor : normalColor;
            itemButtons[i].transform.localScale = i == selected ? Vector3.one * 1.08f : Vector3.one;
        }

        selectedLabel.text = "Colocando: " + placer.SelectedItem.displayName;
    }
}
