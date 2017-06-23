using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private ISpiderWeapon spiderWeapon;
    private int amountOfUpgrades;

    private Loottable loottable;
    private Vector3 startPosition;

    public float currentHealth;
    [SerializeField]
    private float maxHealth = 20f;
    private bool isDead = false;

    public bool IsDead { get { return isDead; } }

    public int level = 1;

    private Vector3 currentTargetPos;               // the current position to walk to
    private Vector3 targetPosition;                 // the target position if in wander state
    private Transform targetTransform;              // the target position of the player when in follow state. 
    private PlayerController targetScript;
    private bool onCooldown;
    private bool onHitCooldown = false;
    private float basicAttackCooldown = 2f;

    private int inBattleRotationSpeed = 4;
    private int wanderRotationSpeed = 1;
    private int currentRotationSpeed { get { return currentTargetPos == targetPosition ? wanderRotationSpeed : inBattleRotationSpeed; } }

    private static FloatRange baseProgression;

    private float noticeDistance = 10f;

    private Transform myTransform;
    private Animator anim;

    [SerializeField]
    private int[] upgrades; // 0 = damage, 1 = health, 2 = speed

    private List<Floor> floorsInMyRoom;

    public void Initialize(int amountOfUpgrades, int level)
    {
        myTransform = transform;
        startPosition = transform.position;

        upgrades = new int[3];
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = 0;
        }

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        anim = GetComponent<Animator>();

        loottable = new Loottable();
        loottable.Initialize(2, 5);

        spiderWeapon = new SpiderEquipment();
        this.amountOfUpgrades = amountOfUpgrades;
        UpgradeEquipment(this.amountOfUpgrades, level);
        baseProgression = new FloatRange(0.05f, 0.1f);

        maxHealth = spiderWeapon.GetHealth();
        currentHealth = maxHealth;

        //finds highest value of upgrades and returns its index
        int bestUpgrade = (upgrades[0] > upgrades[1])
            ? upgrades[0] > upgrades[2] ? 0 : 2
            : upgrades[1] > upgrades[2] ? 1 : 2;
        Renderer r = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        switch (bestUpgrade)
        {
            case 0:
                r.material = Resources.Load("Materials/Monsters/Spider/spider_diffRed") as Material;
                break;
            case 1:
                r.material = Resources.Load("Materials/Monsters/Spider/spider_diffCyan") as Material;
                break;
            case 2:
                r.material = Resources.Load("Materials/Monsters/Spider/spider_diffYellow") as Material;
                break;
        }

        floorsInMyRoom = new List<Floor>();
        floorsInMyRoom = GameManager.Instance.DungeonManager.CurrentDungeon.GetFloorsInRoom(
            GameManager.Instance.DungeonManager.WorldToGridPosition(transform.position));

        SetNewTargetPosition();

        StartCoroutine(HandleMovement());
    }

    private void SetNewTargetPosition()
    {
        int randomFloor = UnityEngine.Random.Range(0, floorsInMyRoom.Count);

        Vector3 newPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(floorsInMyRoom[randomFloor].xPos, floorsInMyRoom[randomFloor].yPos));
        newPos.y = myTransform.position.y;
        if (targetPosition != newPos) {
            targetPosition = newPos;
        }
        else {
            SetNewTargetPosition();
        }
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
                    upgrades[0]++;
                    break;

                // health
                case 1:
                    spiderWeapon = new HealthUpgrade(spiderWeapon, 10 * (level / 5f));
                    upgrades[1]++;
                    break;

                // speed
                case 2:
                    float value = 1f * (level / 5f);
                    value = value >= 5.0f ? 5.0f : value;
                    spiderWeapon = new SpeedUpgrade(spiderWeapon, value);
                    upgrades[2]++;
                    break;

                default:
                    break;
            }
        }

        Debug.Log("Hi, I have " + amount + "upgrades. Damage: " + spiderWeapon.GetDamage() + ". Health: " + spiderWeapon.GetHealth() + ". Speed: " + spiderWeapon.GetSpeed());
    }

    public void Reset()
    {
        this.gameObject.SetActive(true);

        myTransform = transform;
        onCooldown = false;
        isDead = false;

        anim.SetTrigger("Revive");
        anim.SetBool("Dead", false);

        loottable.Initialize(2,5);

        transform.position = startPosition;
        currentHealth = maxHealth;

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        StopCoroutine(HandleMovement());
        StartCoroutine(HandleMovement());
    }

    private IEnumerator HandleMovement()
    {
        while (true)
        {
            if (!onHitCooldown)
            {
                float distanceToPlayer = (targetTransform.transform.position - transform.position).magnitude;
                float direction = Vector3.Dot((targetTransform.position - transform.position).normalized,
                    transform.forward);

                // folow player state
                if (distanceToPlayer < noticeDistance && distanceToPlayer > spiderWeapon.GetAttackRange())
                {
                    currentTargetPos = targetTransform.position;

                    Rotate();
                    Move();
                }
                // attack state
                else if (distanceToPlayer <= spiderWeapon.GetAttackRange() && direction > 0 && !onCooldown)
                {
                    currentTargetPos = targetTransform.position;

                    Attack();
                    StartCoroutine(WaitForCooldown(basicAttackCooldown));
                }
                // wander state
                else
                {
                    currentTargetPos = targetPosition;

                    float distToPoint = (currentTargetPos - transform.position).magnitude;
                    if (distToPoint < 0.5f)
                    {
                        SetNewTargetPosition();
                        anim.SetFloat("MoveZ", 0f);

                        yield return new WaitForSeconds(1);

                        currentTargetPos = targetPosition;
                    }

                    Rotate();
                    Move();
                }
            }
            else
            {
                anim.SetFloat("MoveZ", 0f);
            }

            yield return null;
        }

        yield break;
    }

    private void Rotate()
    {
        // If not looking at target, look at target. 
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(currentTargetPos - myTransform.position), currentRotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        // If target is not reached, move towards target.
        myTransform.position += myTransform.forward * spiderWeapon.GetSpeed() * Time.deltaTime;
        anim.SetFloat("MoveZ", 1f);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        targetScript.GotHit(spiderWeapon.GetDamage(), this);
        AdjustCurrentHealth(-GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Thorns));
    }

    private IEnumerator WaitForCooldown(float cooldownTime)
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;

        yield break;
    }

    private IEnumerator Hitted(float cooldownTime)
    {
        onHitCooldown = true;
        
        yield return new WaitForSeconds(cooldownTime / 2.0f);
        Renderer r = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        Material myMat = r.material;
        r.material = Resources.Load("Materials/Monsters/Spider/spider_hit") as Material;
        yield return new WaitForSeconds(cooldownTime / 2.0f);
        r.material = myMat;


        onHitCooldown = false;

        yield break;
    }

    public void GotHit(float dmg)
    {
        AdjustCurrentHealth(-dmg);
        StartCoroutine(Hitted(1));
    }

    public void AdjustCurrentHealth(float adj)
    {
        if (isDead) return;

        currentHealth += adj;

        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            Die();

            return;
        }

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    private void Die()
    {
        anim.SetBool("Dead", true);
        GameManager.Instance.ActiveCharacterInformation.AddExperiencePoints(baseProgression.GetRandomInRange());

        GameManager.Instance.ActiveCharacterInformation.PlayerController.AdjustCurrentHealth(GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerKill));

        while (!spiderWeapon.IsBase()) { spiderWeapon = spiderWeapon.RemoveUpgrade(); }

        StartCoroutine(WaitForDestory());
    }

    private IEnumerator WaitForDestory()
    {
        yield return new WaitForSeconds(3);

        StopCoroutine(HandleMovement());

        loottable.DropItems(this.transform.position);

        this.gameObject.SetActive(false);
        this.transform.position = startPosition;
        //Destroy(this.gameObject);

        yield break;
    }
}
