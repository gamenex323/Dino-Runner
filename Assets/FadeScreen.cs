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
    public void UpdateTheMap()
    {
        GameManager.Instance.UpdateTheMap();
    }
}
