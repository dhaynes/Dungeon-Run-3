using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [Space(15)]
    [Tooltip("Number of seconds spent in anticipation prior to attack")]
    public float attackAnticipationDuration = 2f;

    [Tooltip("Percent likelihood that an enemy will attack.")]
    public float attackPercentage = 0.5f;

    [Tooltip("Cool tooltip, bro. Percent likelihood that an attack will put the enemy into block mode.")]
    public float blockPercentage = 0.1f;

    [Tooltip("Timer that shows how long until the enemy will open itself up to attack again.")]
    public float blockCooldown;
    public float maxBlockCooldown;

    public BoxCollider attackCollider;

    private Hero _hero;

    [Space(15)]
    public float realignmentSpeed = 0.02f;
    public float realignmentCooldown = 1f;

    void Start()
	{
        //attach anim helper script to mesh
        if (!mesh.GetComponent<AnimEventHelper>())
        {
            mesh.AddComponent<AnimEventHelper>();
        }

        _hero = GameController.instance.hero;

        attackCollider.gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    void Update()
    {
        if (isBlocking)
        {
            blockCooldown -= Time.deltaTime;
            if (blockCooldown < 0)
            {
                blockCooldown = 0;
                BlockCooldownEnded();
            }
        }

        UpdateAlignment();

    }

    public void UpdateAlignment()
    {
        if (realignmentCooldown > 0)
        {
            realignmentCooldown -= Time.deltaTime;
            return;
        }
        else
        {
            realignmentCooldown = 0;
        }

        Vector3 pos = transform.localPosition;
        float distance = Vector3.Distance(pos, Vector3.zero);

        if (pos.z < -realignmentSpeed) //if it has been pushed forwards...
        {
            pos.z += realignmentSpeed;
        }
        else if (pos.z > realignmentSpeed) //if it's towards the back...
        {
            pos.z -= realignmentSpeed;
        }
        else if (pos.z >= -realignmentSpeed && pos.z <= realignmentSpeed) // if it's mostly at rest in the middle...
        {
            pos.z = 0;
        }

        transform.localPosition = pos;

    }

    public void TakeDamage(float damageAmt)
    {
        if (isDead) return;

        if (isAttacking) 
        {
            GameController.instance.damageTextEffect.ShowTextEffect("Missed!", middleOfCollider);
            return;
        }

        //evaluate whether or not this should trigger a block. Once an enemy blocks, it will continue blocking until it attacks again.
        if (AttackBlocked())
        {
            animator.SetTrigger("Block");
            isBlocking = true;

            //show the text effect
            GameController.instance.damageTextEffect.ShowTextEffect("Blocked!", middleOfCollider);

            BeginBlock();

            return;
        }

        if (!invincible) currentHealth -= damageAmt;

        if (currentHealth <= 0)
        {
            //die
            currentHealth = 0;
            Die();
        }
        else
        {
            //update the meter
            float newHealthValue = currentHealth / startingHealth;
            healthMeter.meter.value = newHealthValue;

            animator.SetTrigger("TakeDamage");

            GameController.instance.damageTextEffect.ShowTextEffect("-" + damageAmt, middleOfCollider);

            //knockback effect - only for uppercuts or power attacks
            //rbody.AddForce(transform.forward * 300f);
        }

        bloodFX.Play();
    }


    public bool AttackBlocked()
    {
        bool attackBlocked = false;

        if (isBlocking || Random.value <= blockPercentage)
        {
            attackBlocked = true;
        }

        return attackBlocked;
    }
    public void BeginBlock()
    {
        blockCooldown = maxBlockCooldown;
        //block cooldown is decremented in the Update loop
    }

    public void BlockCooldownEnded()
    {
        isBlocking = false;

        //every time a block ends, attack.
        PrepareToAttack();
    }


    public void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        healthMeter.Hide();

        Invoke("TriggerNextEnemy", 2f);
    }

    private void TriggerNextEnemy()
    {
        GameController.instance.enemyGroup.NextEnemy();
        gameObject.SetActive(false);
    }




    //Make an attempt to initiate an attack.
    //this is called from an anim event on the enemy controller's idle
    public void TryToAttack()
    {
        if (Random.value <= attackPercentage)
        {
            PrepareToAttack();
        }
    }

    private void PrepareToAttack()
    {
        animator.SetTrigger("PrepareToAttack");

        attackCollider.enabled = true;

        isAttacking = true;
        isBlocking = false;
    }

    private void Attack()
    {
        //turn on the attack collider. Otherwise it won't register a successful attack.

        animator.SetTrigger("Attack");
    }

    private void DealHeroDamage()
    {
        _hero.TakeDamage(strength);

        isAttacking = false;
        isBlocking = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy attack collider activated");
            attackCollider.enabled = false;

            //do some damage
            DealHeroDamage();
        }
    }
}
