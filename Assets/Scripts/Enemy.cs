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
    public float blockCooldown = 0f;
    public float maxBlockCooldown = 0f;



    public bool isAttacking
    {
        get { return animator.GetBool("isAttacking"); }
        private set { animator.SetBool("isAttacking", value); }
    }

    public bool isDead
    {
        get { return animator.GetBool("isDead"); }
        private set { animator.SetBool("isDead", value); }
    }

    public bool isBlocking
    {
        get { return animator.GetBool("isBlocking"); }
        private set { animator.SetBool("isBlocking", value); }
    }


    private Rigidbody _rbody;
    private Hero _hero;

    void Start()
	{
        //attach anim helper script to mesh
        mesh.AddComponent<AnimEventHelper>();
        _hero = GameController.instance.hero;
    }

    void Update()
    {
        blockCooldown -= Time.deltaTime;
        if (blockCooldown < 0)
        {
            blockCooldown = 0;
            EndBlock();
        }
    }

    public void Reset()
	{
        healthMeter = GameController.instance.enemyHealthMeter;
        healthMeter.meter.value = 1;
        currentHealth = startingHealth;

        _rbody = this.gameObject.GetComponent<Rigidbody>();
        _rbody.sleepThreshold = 0;
    }

    public void MakeEntrance()
    {
        _rbody.WakeUp();
        healthMeter.Show();

        animator.SetTrigger("Enter");
    }

    public void TakeDamage(float damageAmt)
    {
        if (invincible) return;
        if (animator.GetBool("isDead")) return;
        if (animator.GetBool("isAttacking")) return;

        //evaluate whether or not this should trigger a block. Once an enemy blocks, it will continue blocking until it attacks again.
        if (AttackBlocked())
        {
            animator.SetTrigger("Block");
            isBlocking = true;

            //show the text effect
            GameController.instance.damageTextEffect.ShowTextEffect("Blocked!", textEffectSpawnLocation);

            isBlocking = true;
            BeginBlock();

            return;
        }

        currentHealth -= damageAmt;

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

            GameController.instance.damageTextEffect.ShowTextEffect("-" + damageAmt, textEffectSpawnLocation);
        }

        bloodFX.Play();

    }

    public bool AttackBlocked()
    {
        bool attackBlocked = false;

        if (blockCooldown > 0f)
        {
            attackBlocked = true;
        }
        else if (isBlocking)
        {
            attackBlocked = true;
        }
        else if (Random.value <= blockPercentage)
        {
            attackBlocked = true;
        }
        else
        {
            attackBlocked = false;
        }

        return attackBlocked;
    }
    public void BeginBlock()
    {
        blockCooldown = maxBlockCooldown;
    }

    public void EndBlock()
    {
        isBlocking = false;
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
        Invoke("Attack", attackAnticipationDuration);

        isAttacking = true;
        isBlocking = false;
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

    }



    private void EvaluateAttackSuccess()
    {
        _hero.TakeDamage(strength);
        MarkAttackComplete();
    }

    //this is called from an anim event
    public void MarkAttackComplete()
    {
        isAttacking = false;
        isBlocking = false;
    }
}
