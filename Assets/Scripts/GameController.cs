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
    public Hero hero;

    [Space(15)]
    public EnemyGroup enemyGroup;
    public HealthMeter enemyHealthMeter;
    public HealthMeter playerHealthMeter;
    public StaminaMeter staminaMeter;

    [Space(15)]
    public StatusEffectTextContainer damageTextEffect;
    public ParticleSystem smackFX;

    [Space(15)]
    public ActionButton attackButton;
    public ActionButton dodgeButton;
    public ActionButton jumpButton;

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

        Physics.IgnoreLayerCollision(10, 11);
    }

    public void StartGame()
    {
        Canvas.ShowScreen(Canvas.GameScreen);
        hero.MakeEntrance();
        enemyGroup.InitializeEnemyGroup();
    }

    public void GameOver()
    {
        Canvas.ShowScreen(Canvas.GameOverScreen);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            dodgeButton.TriggerAction();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            attackButton.TriggerAction();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            jumpButton.TriggerAction();
        }
    }


}
