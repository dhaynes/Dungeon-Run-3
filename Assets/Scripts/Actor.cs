using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public GameObject mesh;
    public Animator animator;
    public ParticleSystem bloodFX;
    public HealthMeter healthMeter;

    [Space(15)]
    public bool invincible = false;
    public float startingHealth = 100;
    public float currentHealth;
    public float strength = 10;

    public Vector3 middleOfCollider
    {
        get
        {
            Vector3 location = transform.GetComponent<BoxCollider>().bounds.center;

            //get effect container offset to make sure it displays in front of all characters
            location.z = GameController.instance.damageTextEffect.transform.position.z;

            return location;
        }
    }
}
