using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Settings : MonoBehaviour
{
    public static bool reverseColor = false; //reverses all color
    public static bool monoChrome = false; //makes the game black and white

    public void ReverseColor(bool t)
    {
        reverseColor = t;
    }

    public void MonoChrome(bool t)
    {
        monoChrome = t;
    }

}
