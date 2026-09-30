using UnityEngine;

public class williamreset : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chechPoint.HasSpispin = false;
        chechPoint.HasVineWhip = false;
         chechPoint.HasAcornShild = false;
         chechPoint.HasWalljump = false;
        chechPoint.chackpoint = new Vector3(0,0,0);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
