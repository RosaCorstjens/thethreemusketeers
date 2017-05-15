using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spider : WorldObject, IPoolable
{
    // variables that can be upgraded
    private ISpiderWeapon spiderWeapon;

    private Loottable loottable;

    public Vector3 startPosition;

    public float currentHealth;
    private bool isDead = false;

    public int level = 1;

    public Transform targetTransform;
    public PlayerController targetScript;
    public bool OnHitCooldown;
    private float basicAttackCooldown = 2f;
    public float BasicAttackCooldown { get { return basicAttackCooldown; } }

    private int rotationSpeed = 3;

    private float baseProgression = 0.5f;
    public float BaseProgression { get { return baseProgression; } }

    private float noticeDistance = 10f;
    public float NoticeDistance { get { return noticeDistance; } }
    public float AttackRange { get { return spiderWeapon.GetAttackRange(); } }

    private Transform myTransform;
    private Animator anim;
    public Animator Anim { get { return anim; } }

    private float coolDownTime = 2f;
    public float CoolDownTime { get { return coolDownTime; } }

    private StateMachine<Spider> finiteStateMachine;
    public StateMachine<Spider> FiniteStateMachine { get { return finiteStateMachine; } }

    public void Initialize()
    {
        startPosition = transform.position;

        spiderWeapon = new SpiderEquipment();

        BuildStateMachine();

        gameObject.SetActive(false);
    }

    public void UpgradeEquipment(int amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomroll = UnityEngine.Random.Range(0, 3);

            switch (randomroll)
            {
                // dmg
                case 0:
                    spiderWeapon = new DamageUpgrade(spiderWeapon, 3 * (level / 5f));
                    break;

                // health
                case 1:
                    spiderWeapon = new HealthUpgrade(spiderWeapon, 10 * (level / 5f));
                    break;

                // speed
                case 2:
                    spiderWeapon = new SpeedUpgrade(spiderWeapon, 0.1f * (level / 5f));
                    break;

                default:
                    break;
            }
        }
        Debug.Log("Hi, I have " + amount + "upgrades. Damage: " + spiderWeapon.GetDamage() + ". Health: " + spiderWeapon.GetHealth() + ". Speed: " + spiderWeapon.GetSpeed());
    }

    public void SetPosition(Vector3 position)
    {
        DungeonManager.Instance.SpatialPartitionGrid.RemoveObjectFromGrid(this);
        pos = position;
        previousPos = pos;
        transform.position = position;
        AddToGrid();
    }

    private void BuildStateMachine()
    {
        Dictionary<string, State<Spider>> dictionary = new Dictionary<string, State<Spider>>();

        dictionary.Add("Idle", new EnemyIdleState());
        dictionary.Add("RecievedDamage", new EnemyRecievedDamageState());
        dictionary.Add("Battle", new EnemyBattleState());
        dictionary.Add("Attack", new EnemyAttackState());
        dictionary.Add("Dying", new EnemyDyingState());
        dictionary.Add("Reset", new EnemyResetSate());

        finiteStateMachine = new StateMachine<Spider>(this, new EnemyIdleState(), dictionary);

    }

    public void Activate()
    {
        myTransform = transform;
        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        currentHealth = spiderWeapon.GetHealth();

        loottable = new Loottable();
        loottable.Initialize(2, 5);

        finiteStateMachine.Init();
        FiniteStateMachine.SetState(FiniteStateMachine.PossibleStates["Reset"]);
        myTransform = transform;
        this.transform.position = startPosition;

        isDead = false;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        while (!spiderWeapon.IsBase()) { spiderWeapon = spiderWeapon.RemoveUpgrade(); }
        //Debug.Log("Hi, I removed my upgrades!d Damage: " + spiderWeapon.GetDamage() + ". Health: " + spiderWeapon.GetHealth() + ". Speed: " + spiderWeapon.GetSpeed());

        myTransform = null;
        targetTransform = null;
        targetScript = null;
        anim = null;
        loottable = null;
        //finiteStateMachine = null;

        DungeonManager.Instance.SpatialPartitionGrid.RemoveObjectFromGrid(this);
        pos = this.transform.position;
        previousPos = pos;
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(this);
    }

    public override void Update()
    {
        base.Update();
        FiniteStateMachine.Update();
    }

    public void Rotate()
    {
        // If not looking at target, look at target. 
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetTransform.position - myTransform.position), rotationSpeed * Time.deltaTime);
    }

    public void Move()
    {
        // If target is not reached, move towards target.
        myTransform.position += myTransform.forward * spiderWeapon.GetSpeed() * Time.deltaTime;
        anim.SetFloat("MoveZ", 1f);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        targetScript.GotHit(spiderWeapon.GetDamage(), this);
        AdjustCurrentHealth(-GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Thorns));
    }

    public void GotHit(float dmg)
    {
        FiniteStateMachine.SetState(FiniteStateMachine.PossibleStates["RecievedDamage"]);
        AdjustCurrentHealth(-dmg);
    }

    public void AdjustCurrentHealth(float adj)
    {
        if (isDead) return;

        currentHealth += adj;

        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;

            FiniteStateMachine.SetState(FiniteStateMachine.PossibleStates["Dying"]);

            return;
        }

        if (currentHealth > spiderWeapon.GetHealth()) currentHealth = spiderWeapon.GetHealth();
    }

    public void Die()
    {
        loottable.DropItems(this.transform.position);

        DungeonManager.Instance.CurrentDungeon.Enemies.Remove(this);

        GameManager.Instance.PoolingManager.SpiderObjectPool.Store(this);

        //this.gameObject.SetActive(false);
        //this.transform.position = startPosition;
    }
}
