using UnityEngine;

public class PopIn : MonoBehaviour
{
    [SerializeField] float duration = 0.35f;

    Vector3 targetScale;
    float timer;

    void Awake()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        float s = 1f - Mathf.Pow(1f - t, 3f);
        transform.localScale = targetScale * (s + Mathf.Sin(t * Mathf.PI) * 0.15f);

        if (t >= 1f)
        {
            transform.localScale = targetScale;
            enabled = false;
        }
    }
}
