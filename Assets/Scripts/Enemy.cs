using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyMesh;
    public Animator animator;

    [Space(15)]
    public int Health = 100;

    public void TakeDamage(int damageAmt)
    {
        Health -= damageAmt;

        animator.SetTrigger("TakeDamage");
    }
}
