using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spider : MonoBehaviour, IPoolable
{
    private Loottable loottable;
    public Vector3 startPosition;

    public float currentHealth;
    private float maxHealth = 20f;
    private bool isDead = false;

    public int level = 1;

    public Transform targetTransform;
    public PlayerController targetScript;
    public bool OnHitCooldown;
    private float basicAttackCooldown = 2f;
    public float BasicAttackCooldown { get { return basicAttackCooldown; } }

    private float moveSpeed = 1.5f;
    private int rotationSpeed = 3;

    private int damage = 8;
    private float baseProgression = 0.5f;
    public float BaseProgression { get { return baseProgression; } }

    private float noticeDistance = 10f;
    public float NoticeDistance { get { return noticeDistance; } }
    private float attackDistance = 2f;
    public float AttackDistance { get { return attackDistance; } }

    private Transform myTransform;
    private Animator anim;
    public Animator Anim { get { return anim; } }

    private float coolDownTime = 2f;
    public float CoolDownTime { get { return coolDownTime; } }

    private StateMachine<Spider> finiteStateMachine;
    public StateMachine<Spider> FiniteStateMachine { get { return finiteStateMachine; } }

    public void Initialize()
    {
        //myTransform = transform;
        startPosition = transform.position;

        //targetTransform = GameManager.Instance.ActiveCharacter.transform;
        //targetScript = targetTransform.gameObject.GetComponent<PlayerController>();
        //anim = GetComponent<Animator>();
        //currentHealth = maxHealth;
        //loottable = new Loottable();
        //loottable.Initialize(2, 5);

        Dictionary<string, State<Spider>> dictionary = new Dictionary<string, State<Spider>>();

        dictionary.Add("Idle", new EnemyIdleState());
        dictionary.Add("RecievedDamage", new EnemyRecievedDamageState());
        dictionary.Add("Battle", new EnemyBattleState());
        dictionary.Add("Attack", new EnemyAttackState());
        dictionary.Add("Dying", new EnemyDyingState());
        dictionary.Add("Reset", new EnemyResetSate());

        finiteStateMachine = new StateMachine<Spider>(this, new EnemyIdleState(), dictionary);

        gameObject.SetActive(false);
    }

    public void Activate()
    {
        myTransform = transform;
        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        loottable = new Loottable();
        loottable.Initialize(2, 5);

        finiteStateMachine.Init();
        FiniteStateMachine.SetState(FiniteStateMachine.PossibleStates["Reset"]);
        myTransform = transform;
        this.transform.position = startPosition;
        isDead = false;

        gameObject.SetActive(true);
    }

    /*public void Reset()
    {
        FiniteStateMachine.SetState(FiniteStateMachine.PossibleStates["Reset"]);

        myTransform = transform;
        isDead = false;

        loottable.Initialize(2, 5);

        currentHealth = maxHealth;
    }*/

    public void Deactivate()
    {
        myTransform = null;
        targetTransform = null;
        targetScript = null;
        anim = null;
        loottable = null;
        //finiteStateMachine = null;

        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(this);
    }

    void Update()
    {
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
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        anim.SetFloat("MoveZ", 1f);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        targetScript.GotHit(damage, this);
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

        if (currentHealth > maxHealth) currentHealth = maxHealth;
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
