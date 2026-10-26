using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TargetStatusLabel : MonoBehaviour
{
    [SerializeField] ObserverBehaviour target;
    [SerializeField] Text label;
    [SerializeField] string searchingText = "Apuntá la cámara al target";
    [SerializeField] string foundText = "Target encontrado";
    [SerializeField] Color searchingColor = new Color(1f, 0.85f, 0.3f);
    [SerializeField] Color foundColor = new Color(0.4f, 1f, 0.5f);

    void Start()
    {
        if (target != null)
        {
            target.OnTargetStatusChanged += HandleStatusChanged;
            HandleStatusChanged(target, target.TargetStatus);
        }
        else
        {
            SetSearching();
        }
    }

    void OnDestroy()
    {
        if (target != null)
            target.OnTargetStatusChanged -= HandleStatusChanged;
    }

    void HandleStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            label.text = foundText;
            label.color = foundColor;
        }
        else
        {
            SetSearching();
        }
    }

    void SetSearching()
    {
        label.text = searchingText;
        label.color = searchingColor;
    }
}
