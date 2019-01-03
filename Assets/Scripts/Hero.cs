using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Actor
{
    [Space(15)]
    public int jumpHeight = 740;

    private Rigidbody _rbody;

    public bool isDodging
    { 
        get => animator.GetBool("isDodging");
        set => animator.SetBool("isDodging", value); 
    }

    public bool isJumping
    {
        get => animator.GetBool("isJumping");
        set => animator.SetBool("isJumping", value);
    }



    private void Awake()
    {
        _rbody = this.GetComponent<Rigidbody>();
        animator = mesh.GetComponent<Animator>();
    }

    void Start()
	{
        _rbody.sleepThreshold = 0;

        Reset();
	}

    public void Reset()
    {
        mesh.SetActive(false);

        healthMeter.meter.value = 1;
        
        //reset stats
        currentHealth = startingHealth;
        isDodging = false;
        animator.SetBool("isJumping", false);
        animator.Play("Hidden", -1, 0f);
    }

    public void MakeEntrance()
	{
        mesh.SetActive(true);
        animator.SetTrigger("Enter");
        healthMeter.Show();

        //do this so that the rigidbody responds to ground collision.
        _rbody.WakeUp();
    }

	public void Attack()
    {
        animator.SetTrigger("Attack");

        if (IsUppercut())
        {
            //_rbody.AddForce(Vector3.up * jumpHeight);
        }

    }

    private void DealEnemyDamage()
    {
        GameController.instance.enemyGroup.currentEnemy.TakeDamage(GameController.instance.hero.strength);
    }

    public bool IsUppercut()
    {
        return isDodging;
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
        isJumping = true;

        _rbody.velocity = Vector3.zero;
        _rbody.AddForce(Vector3.up * jumpHeight);
    }

    public void Float()
    {
        _rbody.AddForce(Vector3.up * 200);
    }

    public void Dodge()
    {
        animator.SetTrigger("Dodge");
        isDodging = true;
    }

    public void MarkDodgeComplete()
    {
        isDodging = false;
        Debug.Log("Dodge complete");
    }

    public void TakeDamage(float val)
    {
        if (invincible) return;
        if (isDodging == false)
        {
            currentHealth -= val;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            else
            {
                animator.SetTrigger("TakeDamage");
            }

            //show the damage text effect
            GameController.instance.damageTextEffect.ShowTextEffect("-" + val, middleOfCollider);

            //update the meter to new health value
            float newHealthValue = currentHealth / startingHealth;
            healthMeter.meter.value = newHealthValue;

            bloodFX.Play();
        }
    }

    public void Die()
    {
        Debug.Log("You are dead!");
        animator.SetTrigger("Die");
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Ground")
        {
            //Debug.Log("Ground!");
            animator.SetBool("isJumping", false);
            isJumping = false;
        }
	}
}
