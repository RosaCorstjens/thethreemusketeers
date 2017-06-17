using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private ISpiderWeapon spiderWeapon;
    private int amountOfUpgrades;

    private Loottable loottable;
    private Vector3 startPosition;

    public float currentHealth;
    private float maxHealth = 20f;
    private bool isDead = false;

    public int level = 1;

    private Transform targetTransform;
    private PlayerController targetScript;
    private bool onCooldown;
    private bool onHitCooldown = false;
    private float basicAttackCooldown = 2f;

    private float moveSpeed = 1.5f;
    private int rotationSpeed = 3;

    private static FloatRange baseProgression;

    private float noticeDistance = 10f;

    private Transform myTransform;
    private Animator anim;

    public void Initialize(int amountOfUpgrades, int level)
    {
        myTransform = transform;
        startPosition = transform.position;

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        anim = GetComponent<Animator>();

        currentHealth = maxHealth;

        loottable = new Loottable();
        loottable.Initialize(2, 5);

        spiderWeapon = new SpiderEquipment();
        this.amountOfUpgrades = amountOfUpgrades;
        UpgradeEquipment(this.amountOfUpgrades, level);
        baseProgression = new FloatRange(0.05f, 0.1f);
        
        StartCoroutine(HandleMovement());
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
                float distance = (targetTransform.transform.position - transform.position).magnitude;
                float direction = Vector3.Dot((targetTransform.position - transform.position).normalized,
                    transform.forward);

                if (distance < noticeDistance && distance > spiderWeapon.GetAttackRange())
                {
                    Rotate();

                    Move();
                }
                else if (distance <= spiderWeapon.GetAttackRange() && direction > 0 && !onCooldown)
                {
                    Attack();
                    StartCoroutine(WaitForCooldown(basicAttackCooldown));
                }
                else
                {
                    anim.SetFloat("MoveZ", 0f);
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
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetTransform.position - myTransform.position), rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        // If target is not reached, move towards target.
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
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

        yield return new WaitForSeconds(cooldownTime);

        onHitCooldown = false;

        yield break;
    }

    public void GotHit(float dmg)
    {
        AdjustCurrentHealth(-dmg);
        StartCoroutine(Hitted(2));
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
        GameManager.Instance.ActiveCharacterInformation.AddExperiencePoints(baseProgression.GetRandomInRange() * amountOfUpgrades);

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
