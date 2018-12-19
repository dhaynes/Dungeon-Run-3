using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenManager : MonoBehaviour
{
    [Header("Game Screens")]
    public GameObject StartScreen;
    public GameObject GameScreen;


    void HideAll()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            GameObject go = this.gameObject.transform.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    public void ShowScreen(GameObject screen)
    {
        HideAll();
        screen.SetActive(true);
    }
}
