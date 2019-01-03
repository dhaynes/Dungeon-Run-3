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

    public GameObject shield;

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
        private set { animator.SetBool("isBlocking", value); shield.SetActive(value); }
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
        if (isBlocking)
        {
            blockCooldown -= Time.deltaTime;
            if (blockCooldown < 0)
            {
                blockCooldown = 0;
                BlockCooldownEnded();
            }
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

        //clear the dodge and attacking flags
        ClearFlags();
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
            GameController.instance.damageTextEffect.ShowTextEffect("Blocked!", middleOfCollider);

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

            GameController.instance.damageTextEffect.ShowTextEffect("-" + damageAmt, middleOfCollider);
        }

        bloodFX.Play();
        ShowSmackEffect();
    }

    private void ShowSmackEffect()
    {
        //smack location
        Vector3 smackLocation = middleOfCollider;
        //offset it up to the upper left corner of the collider, and
        smackLocation.x -= 0.6f;
        smackLocation.y += 0.6f;
        smackLocation.z = GameController.instance.smackFX.transform.parent.position.z;
        GameController.instance.smackFX.transform.position = smackLocation;
        GameController.instance.smackFX.Play();
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
        ClearFlags();
    }

    //this is called from an anim event
    public void ClearFlags()
    {
        isAttacking = false;
        isBlocking = false;
    }
}
