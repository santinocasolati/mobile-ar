using UnityEngine;
using Vuforia;

public class TargetContent : MonoBehaviour
{
    [SerializeField] float spinSpeed = 40f;
    [SerializeField] float popDuration = 0.4f;
    [SerializeField] float bobHeight = 0.01f;
    [SerializeField] float bobSpeed = 2f;

    ObserverBehaviour target;
    Vector3 baseScale;
    Vector3 basePosition;
    float popTimer;
    bool visible;

    void Awake()
    {
        target = GetComponentInParent<ObserverBehaviour>();
        baseScale = transform.localScale;
        basePosition = transform.localPosition;
        transform.localScale = Vector3.zero;
    }

    void OnEnable()
    {
        if (target != null)
            target.OnTargetStatusChanged += HandleStatusChanged;
    }

    void OnDisable()
    {
        if (target != null)
            target.OnTargetStatusChanged -= HandleStatusChanged;
    }

    void HandleStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool tracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

        if (tracked && !visible)
            popTimer = 0f;

        visible = tracked;
    }

    void Update()
    {
        if (!visible)
        {
            transform.localScale = Vector3.zero;
            return;
        }

        popTimer += Time.deltaTime;
        float t = Mathf.Clamp01(popTimer / popDuration);
        transform.localScale = baseScale * EaseOutBack(t);

        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
        transform.localPosition = basePosition + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight;
    }

    static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
