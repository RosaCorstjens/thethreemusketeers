﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
//[RequireComponent(typeof(PlayerInformation))]

public class PlayerController : MonoBehaviour
{
    public float currentHealth;

    private Transform hand;
    private Transform offhand;
    private WeaponInstance inHand;
    private ShieldInstance inOffHand;

    private bool inBattle;
    private EnemyController targetToAttack;
    private bool onCooldown;
    private float basicAttackCooldown = 1f;

    private float maxPassiveInBattle = 10f;

    private float turnInput, turnSpeed = 100f;

    //public bool CanMove { get; set; }
    public bool IsInitialized { get; set; }

    // Use this for initialization
    public void Initialize(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;

        currentHealth = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth;

        hand = transform.FindChild(GameManager.Instance.ActiveCharacterInformation.CharacterClass.CharacterClassType.ToString() + "/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandMiddle1/RightHandMiddle2/RightHandMiddle3/RightHandMiddle4");
        inHand = null; // Start with nothing in hand. 

        //offhand = transform.FindChild("PelvisRoot/Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftArm/LeftArmRoll/LeftForeArm/LeftForeArmRoll/LeftHand/OffHand");
        inOffHand = null;

        IsInitialized = true;
    }

    private void Update()
    {
        if (!IsInitialized) return;

        GetInput();

        Turn();
    }

    private void GetInput()
    {
        SendMessage("MoveMeForward", Input.GetAxis("Forward"));
        //SendMessage("TurnMe", Input.GetAxis("Turn"));
        SendMessage("MoveMeSideways", Input.GetAxis("Strafe"));
        turnInput = Input.GetAxis("Turn");

        if (Input.GetKeyDown(KeyCode.LeftShift)) SendMessage("ToggleRun");

        if (Input.GetMouseButtonDown(0) && !inBattle)
        {
            if (inHand == null)
            {
                GameManager.Instance.UIManager.EquipWeaponWarning();
                return;
            }

            inBattle = true;
            inHand.WeaponObject.SetActive(true);
            StartCoroutine(CheckForLeavingBattle());
            SendMessage("ToggleBattle", true);
        }
        else if (Input.GetMouseButtonDown(0) && inBattle && !onCooldown)
        {
            targetToAttack = FindObjectOfType<EnemyController>();

            float distance = (targetToAttack.transform.position - transform.position).magnitude;
            float direction = Vector3.Dot((targetToAttack.transform.position - transform.position).normalized, transform.forward);

            if(distance < 2.5f)
            {
                if(direction > 0)
                {
                    Attack();
                }
            }

            StartCoroutine(WaitForCooldown(basicAttackCooldown));
            SendMessage("BasicAttack");
        }
    }

    private void Turn()
    {
        transform.Rotate(new Vector3(0, turnSpeed * turnInput * Time.deltaTime));
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

    private void SeekTarget()
    {
        float distance = Vector3.Distance(targetToAttack.transform.position, transform.position);

        if (distance < 10) Attack();
    }

    private void Attack()
    {
        // TO DO: take crit dmg etc into account. 
        targetToAttack.AdjustCurrentHealth(GameManager.Instance.ActiveCharacterInformation.Stats.DeterminedDamage);
    }

    private IEnumerator WaitForCooldown(float cooldownTime)
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;

        yield break;
    }

    public void AdjustCurrentHealth(float adj)
    {
        currentHealth -= adj;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }

        if (currentHealth > GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth) currentHealth = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth;

        GameManager.Instance.UIManager.HudManager.HealthBar.SetBar(currentHealth, GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth);
    }

    private void Die()
    {
        // DEAD TO THE PLAAAYER
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
