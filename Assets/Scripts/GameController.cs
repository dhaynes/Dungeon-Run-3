using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController _instance;

    [Space(15)]
    public GameObject World;
    public UIScreenManager Canvas;

    [Space(15)]
    public Hero Hero;
    public Enemy Enemy;

    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Canvas.ShowScreen(Canvas.StartScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
