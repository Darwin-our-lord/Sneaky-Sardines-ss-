using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject monoChrome;
    public GameObject reverseColor;
    void Awake()
    {
        if(Settings.monoChrome) monoChrome.SetActive(true);
        else if(!Settings.monoChrome) monoChrome.SetActive(false);
        if (Settings.reverseColor) reverseColor.SetActive(true);
        else if (!Settings.reverseColor) reverseColor.SetActive(false);
    }
}
