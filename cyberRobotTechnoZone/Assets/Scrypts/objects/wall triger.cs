using System.Collections;
using UnityEngine;

public class walltriger : MonoBehaviour
{
    public static bool dragerErActiv = false;
    [SerializeField] float targetOrthographicSize = 25f;
    [SerializeField] float zoomDuration = 1.0f;
    Coroutine zoomCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragerErActiv = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dragerErActiv = true;
            var cam = collision.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
                zoomCoroutine = StartCoroutine(ZoomCamera(cam, cam.orthographicSize, targetOrthographicSize, zoomDuration));
            }
            Debug.Log("du burgt være skremt");
        }

    }

    IEnumerator ZoomCamera(Camera cam, float fromSize, float toSize, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cam.orthographicSize = Mathf.Lerp(fromSize, toSize, t);
            yield return null;
        }
        cam.orthographicSize = toSize;
        zoomCoroutine = null;
    }
}
