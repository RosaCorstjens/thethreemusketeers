using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
//[RequireComponent(typeof(PlayerInformation))]

public class PlayerController : WorldObject, IControlable
{
    public float currentHealth;

    private Transform hand;
    private Transform offhand;
    private WeaponInstance inHand;
    private ShieldInstance inOffHand;

    private bool inMenu = false;

    public bool InMenu
    {
        get { return inMenu; }
        set { inMenu = value; }
    }
    private bool inBattle;
    private Spider targetToAttack;
    private bool onCooldown;
    private float basicAttackCooldown = 0;

    private static float baseRunSpeed = 4f;
    private static float baseWalkSpeed = 2f;
    private bool isRunning = true;

    private bool potionOnCooldown;
    private float healthPotionCooldown = 2f;

    private bool regenOnCooldown = false;
    private float regenCooldown = 5f;
    private Coroutine regenCoroutine;
    private bool onHitCooldown = false;

    private float maxPassiveInBattle = 30f;

    private float turnInput, turnSpeed = 150f;

    public bool CanMove { get; set; }
    public bool IsInitialized { get; set; }

    private InputHandler inputHandler;

    // Use this for initialization
    public void Initialize(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        pos = transform.position;
        previousPos = pos;

        currentHealth = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth;

        hand = transform.FindChild(GameManager.Instance.ActiveCharacterInformation.CharacterClass.CharacterClassType.ToString() + "/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandMiddle1/RightHandMiddle2/RightHandMiddle3/RightHandMiddle4");
        inHand = null; // Start with nothing in hand. 

        inOffHand = null;
        CanMove = true;

        inputHandler = new InputHandler(this);

        IsInitialized = true;
    }

    public override void Update()
    {
        if (!IsInitialized) return;

        base.Update();

        if (onHitCooldown)
        {
            SendMessage("MoveMeForward", 0f);
            SendMessage("MoveMeSideways", 0f);
            return;
        }

        //regen
        if (!regenOnCooldown)
        {
            float regen = ((GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.MaxHealth) / 100) * GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerSec)) * Time.deltaTime;
            AdjustCurrentHealth(regen);   
        }

        inputHandler.HandleInput();
    }

    public void Turn(float input)
    {
        if (onCooldown || inMenu || !CanMove) return;
        
        transform.Rotate(new Vector3(0, turnSpeed * input * Time.deltaTime));
    }

    public void Move(float input, string animation, Vector3 direction)
    {
        if (onCooldown || inMenu || !CanMove) return;

        input *= isRunning ? baseRunSpeed : baseWalkSpeed;

        SendMessage(animation, input);

        transform.Translate(direction * input * 
            GameManager.Instance.ActiveCharacterInformation.Stats.DeterminedSpeedMultiplier * 
            Time.deltaTime, Space.Self);
    }

    public void Attack()
    {
        if (!inBattle)
        {
            if (inHand == null)
            {
                UIManager.Instance.EquipWeaponWarning();
                return;
            }

            inBattle = true;
            inHand.WeaponObject.SetActive(true);
            StartCoroutine(CheckForLeavingBattle());
            SendMessage("ToggleBattle", true);
        }
        else if(!onCooldown)
        {
            targetToAttack = FindTarget();

            if(targetToAttack != null) DealDamage();

            StartCoroutine(WaitForCooldown(basicAttackCooldown));
            SendMessage("BasicAttack");
        }
    }

    private Spider FindTarget()
    {
        List<WorldObject> possibleEnemies = DungeonManager.Instance.SpatialPartitionGrid.GetObjectsAt(DungeonManager.Instance.SpatialPartitionGrid.GetCellAt(transform.position));

        foreach (WorldObject enemy in DungeonManager.Instance.CurrentDungeon.Enemies)
        {
            if (enemy == this) break;

            if (enemy.gameObject.activeInHierarchy)
            {
                // distance check
                float distance = (enemy.transform.position - transform.position).magnitude;
                if (distance < 2.5f)
                {
                    float direction = Vector3.Dot((enemy.transform.position - transform.position).normalized, transform.forward);
                    if (direction > 0)
                    {
                        return (Spider)enemy;
                    }
                }
            }
        }

        return null;
    }

    private void DealDamage()
    {
        float dmg = GameManager.Instance.ActiveCharacterInformation.Stats.DeterminedDamage;
        float randomroll = Random.Range(0, 100);
        if (randomroll < GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.CritRate))
        {
            dmg = (dmg / 100) * GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.CritDamage);
        }

        dmg += GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.WeaponDamage);

        targetToAttack.AdjustCurrentHealth(-dmg);
        AdjustCurrentHealth(GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerHit));
    }

    public void SwitchRun()
    {
        isRunning = !isRunning;
        SendMessage("ToggleRun");
    }

    public void UsePotion()
    {
        if (potionOnCooldown) return;

        UIManager.Instance.InventoryManager.UsePotion(PotionType.Health);
    }

    private IEnumerator CheckForLeavingBattle()
    {
        float startTime = Time.time;
        float endTime = Time.time + maxPassiveInBattle;

        while (startTime < endTime)
        {
            startTime += Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                startTime = Time.time;
                endTime = Time.time + maxPassiveInBattle;
            }

            yield return null;
        }

        inBattle = false;
        inHand.WeaponObject.SetActive(false);
        SendMessage("ToggleBattle", false);

        yield break;
    }

    private IEnumerator WaitForCooldown(float cooldownTime)
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;

        yield break;
    }

    private IEnumerator PauseRegen(float cooldownTime)
    {
        regenOnCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        regenOnCooldown = false;

        regenCoroutine = null;
        yield break;
    }

    public void GotHit(float dmg, Spider enemy)
    {
        // adjust dmg based on armor
        dmg -= GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Armor) /
              (50 * enemy.level + GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Armor));
        
        // if shield, adjust dmg based on block chance and amount
        if (inOffHand)
        {
            float randomroll = Random.Range(0, 100);
            if (randomroll < GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.BlockChance))
            {
                dmg -= GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.BlockAmount);
            }
        }

        AdjustCurrentHealth(-dmg);

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
        regenCoroutine = StartCoroutine(PauseRegen(regenCooldown));

        StartCoroutine(Hitted(1));
    }

    private IEnumerator Hitted(float cooldownTime)
    {
        onHitCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onHitCooldown = false;

        yield break;
    }

    public void AdjustCurrentHealth(float adj)
    {
        currentHealth += adj;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
        else if(currentHealth > GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth)
        {
            currentHealth = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth;
        }

        if (currentHealth > GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth) currentHealth = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth;

        UIManager.Instance.HudManager.HealthBar.SetBar(currentHealth, GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth);
    }

    private void Die()
    {
        // dungeon monsters respawn
        StartCoroutine(Respawn(DungeonManager.Instance.CurrentDungeon.StartPosition));

        DungeonManager.Instance.CurrentDungeon.RestartDungeon();

        UIManager.Instance.YouDiedWarning();
    }

    public IEnumerator Respawn(Vector3 pos)
    {
        onCooldown = true;
        inBattle = false;
        CanMove = false;
        currentHealth = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth;

        transform.position = pos;

        yield return new WaitForSeconds(3);

        onCooldown = false;
        inBattle = false;
        CanMove = true;
    }

    public void SetHand(WeaponInstance item)
    {
        if (inHand != null)
        {
            RemoveFromHand();

            switch (inHand.BaseWeapon.WeaponType)
            {
                case WeaponType.Bow:
                    SendMessage("ToggleBow", false);
                    break;
                case WeaponType.Staff:
                    SendMessage("ToggleStaff", false);
                    break;
                case WeaponType.Sword:
                    SendMessage("ToggleSword", false);
                    break;
            }
        }

        inHand = item;
        basicAttackCooldown = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.AttackSpeed);
        Debug.Log(basicAttackCooldown);

        inHand.WeaponObject.transform.SetParent(hand);
        inHand.WeaponObject.transform.localRotation = hand.localRotation * Quaternion.Euler(0, 0, 45) * Quaternion.Euler(0, -180, 0);
        inHand.WeaponObject.transform.localPosition = new Vector3(0, 0, 0);

        inHand.WeaponObject.SetActive(false);

        switch (inHand.BaseWeapon.WeaponType)
        {
            case WeaponType.Bow:
                SendMessage("ToggleBow", true);
                break;
            case WeaponType.Staff:
                SendMessage("ToggleStaff", true);
                break;
            case WeaponType.Sword:
                SendMessage("ToggleSword", true);
                break;
        }
    }

    public void RemoveFromHand()
    {
        inHand.WeaponObject.transform.SetParent(GameManager.Instance.gameObject.transform);
        inHand.WeaponObject.SetActive(false);

        inHand = null;
        basicAttackCooldown = 0;
    }

    public void SetOffHand(ShieldInstance item)
    {
        if (inOffHand != null)
        {
            RemoveFromOffHand();
        }

        inOffHand = item;

        inOffHand.ShieldObject.transform.SetParent(offhand);
        inOffHand.ShieldObject.transform.localPosition = new Vector3(0, 0, 0);
        inOffHand.ShieldObject.transform.localRotation = hand.localRotation;

        inOffHand.ShieldObject.SetActive(true);
    }

    public void RemoveFromOffHand()
    {
        inOffHand.ShieldObject.transform.SetParent(GameManager.Instance.gameObject.transform);
        inOffHand.ShieldObject.SetActive(false);

        inOffHand = null;
    }
}
