using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SurfaceHint : MonoBehaviour
{
    [SerializeField] Text hintText;
    [SerializeField] string searchingMessage = "Mové el celular despacio para detectar el piso";
    [SerializeField] string readyMessage = "Tocá el piso para colocar el objeto";
    [SerializeField] float lostDelay = 0.5f;

    float lastHitTime = -10f;

    public void OnSurfaceHit(HitTestResult hit)
    {
        if (hit != null)
            lastHitTime = Time.time;
    }

    void Update()
    {
        bool surfaceFound = Time.time - lastHitTime < lostDelay;
        hintText.text = surfaceFound ? readyMessage : searchingMessage;
    }
}
