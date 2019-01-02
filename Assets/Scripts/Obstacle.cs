using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool isDead = true;
    public bool isGround = true;
    public float Speed = 20f;
    public Collider col;
    public MeshRenderer meshRenderer;

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        float positionX = this.transform.position.x - (Speed / 100f);
        Vector3 newPosition = new Vector3(positionX, 0, 0);
        transform.position = newPosition;
    }

}
