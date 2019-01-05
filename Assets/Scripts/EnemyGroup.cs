using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public List<Enemy> enemiesToLoad = new List<Enemy>();

    [Space(15)]
    public int currentEnemyIndex = 0;
    public Enemy currentEnemy;

    [Space(15)]
    [SerializeField]
    private List<Enemy> enemiesLoaded = new List<Enemy>();



    void Start()
    {
        //make sure there are no stray gameobjects lying about
        ClearGroup();
    }

    private void ClearGroup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            {
                Destroy(go);
            }
        }
    }

    public void InitializeEnemyGroup()
    {
        currentEnemyIndex = -1;
        Debug.Log("Enemy Group Initialized. Enemies in group: " + enemiesToLoad.Count.ToString());

        //empty the loaded enemies array, and the Enemy Group container.
        enemiesLoaded.Clear();
        ClearGroup();

        //clone all enemies
        for (int i = 0; i < enemiesToLoad.Count; i++)
        {
            //create new enemies. They won't be shown until NextEnemy() is called.
            Enemy e = enemiesToLoad[i];
            Enemy newEnemy = Instantiate(Resources.Load<Enemy>("Enemies/" + e.gameObject.name));
            enemiesLoaded.Add(newEnemy);
            newEnemy.transform.parent = this.transform;
            newEnemy.transform.localPosition = Vector3.zero;
            newEnemy.transform.localRotation = Quaternion.identity;
            newEnemy.gameObject.SetActive(false);
        }

        NextEnemy();
    }

    public void NextEnemy()
    {
        currentEnemyIndex++;
        if (currentEnemyIndex >= enemiesLoaded.Count)
        {
            EnemyGroupDefeated();
            return;
        }

        //grab the next enemy, and initialize it.
        Enemy currentEnemy = enemiesLoaded[currentEnemyIndex];
        currentEnemy.healthMeter = GameController.instance.enemyHealthMeter;
        currentEnemy.gameObject.SetActive(true);
        currentEnemy.MakeEntrance();

        this.currentEnemy = currentEnemy;
    }

    private void EnemyGroupDefeated()
    {
        GameController.instance.GameOver();
    }
}
