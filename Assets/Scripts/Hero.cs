using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Actor
{
    [Space(15)]
    public int jumpHeight = 740;
    public float forwardAttackForce = 650f;
    public BoxCollider attackCollider;


    void Start()
	{
        healthMeter = GameController.instance.playerHealthMeter;
        attackCollider.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        UpdateAttackCooldown();
    }

    public void Attack()
    {
        //enable the attack collider
        attackCollider.enabled = true;

        animator.SetTrigger("Attack");

        Vector3 attackStartPos = transform.localPosition;
        if (attackStartPos.z > 0)
        {
            attackStartPos.z = 0;
            transform.localPosition = attackStartPos;
        }

        isDodging = false;

    }

    private void UpdateAttackCooldown()
    {
        //if (_attackCooldown <= 0)
        //{
        //    _attackCooldown = 0;
        //    return;
        //}
        //_attackCooldown -= Time.deltaTime;


    }

    private void DealEnemyDamage()
    {
        if (GameController.instance.enemyGroup.currentEnemy == null) return;
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
        animator.SetTrigger("Dodge");
        isDodging = true;

        //Vector3 dodgeStartPos = transform.localPosition;
        //dodgeStartPos.z = 0;
        //transform.localPosition = dodgeStartPos;

        //rbody.AddForce(transform.forward * 1500f);

    }

    public void MarkDodgeComplete()
    {
        isDodging = false;
        Debug.Log("Dodge complete");
    }

    public void TakeDamage(float val)
    {
        if (isDodging) 
        {
            //show the damage text effect
            GameController.instance.damageTextEffect.ShowTextEffect("Dodged!", middleOfCollider);
            return;
        }
            

        if (!invincible) currentHealth -= val;

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
            if (isDodging) return;

            animator.SetTrigger("LandOnGround");
            isJumping = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Attack collider activated");
            attackCollider.enabled = false;

            //do some damage
            DealEnemyDamage();
        }
    }


}
