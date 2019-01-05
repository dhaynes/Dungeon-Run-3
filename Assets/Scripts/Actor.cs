using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public GameObject mesh;
    public Animator animator;
    public ParticleSystem bloodFX;
    public HealthMeter healthMeter;
    public Rigidbody rbody;

    [Space(15)]
    public bool invincible = false;
    public float startingHealth = 100;
    public float currentHealth;
    public float strength = 10;

    public bool isAttacking;
    public bool isDead;
    public bool isBlocking;
    public bool isDodging;
    public bool isJumping;

    private void Awake()
    {
        rbody = this.GetComponent<Rigidbody>();
        rbody.sleepThreshold = 0;
        animator = mesh.GetComponent<Animator>();
    }

    public void MakeEntrance()
    {
        Reset();

        mesh.SetActive(true);
        animator.SetTrigger("Enter");
        healthMeter.Show();

        //do this so that the rigidbody responds to ground collision.
        rbody.WakeUp();

        Debug.Log("Entrance Made: " + gameObject.name);
    }

    public void Reset()
    {
        healthMeter.meter.value = 1;
        currentHealth = startingHealth;

        ClearFlags();

        animator.Play("Hidden", -1, 0f);
    }

    private void ClearFlags()
    {
        isAttacking = false;
        isDead = false;
        isBlocking = false;
        isDodging = false;
        isJumping = false;
    }

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
