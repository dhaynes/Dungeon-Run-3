using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Obstacle obstacle;
    private Obstacle _newObstacle;

    private float timer;
    public float SpawnRate;

    // Start is called before the first frame update
    void Start()
    {
        _newObstacle = obstacle;

        if (obstacle.isGround)
        {

        }

        //start with one right out the gate?
        timer = SpawnRate;

    }




    public void Update()
    {
        timer += Time.deltaTime;

        if (_newObstacle.transform.localPosition.x <= 0)
        {
            SpawnObstacle();
        }


    }

    public void SpawnObstacle()
    {
        //get the length to see how long until a new piece can spawn to create a seamless effect.
        MeshRenderer meshRenderer = _newObstacle.meshRenderer;
        float length = Mathf.Abs(meshRenderer.bounds.max.x - meshRenderer.bounds.min.x);

        if (obstacle.isGround)
        {
            //grab offset to make sure seamless placement
            float offsetX = _newObstacle.transform.localPosition.x;

            //create a new obstacle, and position it at the very end of the obstacle chain, linking it up seamlessly.
            _newObstacle = GameObject.Instantiate(obstacle, this.transform, true);
            _newObstacle.gameObject.SetActive(true);
            _newObstacle.transform.localPosition = new Vector3(length + offsetX, 0, 0);
            _newObstacle.isDead = false;
        }


    }

}
