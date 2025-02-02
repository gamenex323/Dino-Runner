using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public void FalseFadeScreen()
    {
        gameObject.SetActive(false);
        

    }
    public void isRun()
    {
        Player.instance.isStop = false;
    }
    public void UpdateTheMap()
    {
        //Player.instance.isStop = true;
        //GameManager.Instance.DestroyAllHurdles();
        GameManager.Instance.UpdateTheMap();
    }
}
