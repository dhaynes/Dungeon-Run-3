using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Actor
{
    [Space(15)]
    public int jumpHeight = 740;

    void Start()
	{
        healthMeter = GameController.instance.playerHealthMeter;
	}

    void Update()
    {
        float realignmentSpeed = 0.01f;
        float ease = 0.1f;

        Vector3 pos = transform.localPosition;
        float distance = Vector3.Distance(pos, Vector3.zero);

        if (pos.z < -realignmentSpeed) //if it has been pushed forwards...
        {

            pos.z += distance * ease;
           
        }
        else if (pos.z > realignmentSpeed) //if it's towards the back...
        {
           
            pos.z -= realignmentSpeed;

        }
        else if (pos.z >= -realignmentSpeed && pos.z <= realignmentSpeed)
        {
            pos.z = 0;
        }
        transform.localPosition = pos;

    }

    public void Attack()
    {
        //animator.SetTrigger("Attack");
        //transform.localPosition.Set(transform.localPosition.x, transform.localPosition.y, 0f);
        Vector3 attackStartPos = transform.localPosition;
        attackStartPos.z = 0;
        transform.localPosition = attackStartPos;
        rbody.AddForce(-transform.forward * 2000f);
        isDodging = false;
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

        rbody.velocity = Vector3.zero;
        rbody.AddForce(Vector3.up * jumpHeight);
    }

    public void Dodge()
    {
        //animator.SetTrigger("Dodge");
        isDodging = true;

        rbody.AddForce(transform.forward * 1000f);
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

    //handle collision w/ the ground.
	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetTrigger("LandOnGround");
            isJumping = false;
        }
	}
}
