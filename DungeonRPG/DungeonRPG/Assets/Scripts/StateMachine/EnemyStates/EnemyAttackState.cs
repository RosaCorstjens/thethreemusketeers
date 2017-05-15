﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State<Spider>
{
    bool onCooldown;

    //called once when entering state
    public override void Enter(Spider agent)
    {
        onCooldown = false;
        return;
    }

    //called every frame
    public override void Execute(Spider agent)
    {
        float distance = (agent.targetTransform.transform.position - agent.transform.position).magnitude;
        
        if (distance > agent.AttackRange)
        {
            agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Battle"]);
        }
        else if (!onCooldown)
        {
            agent.Attack();
            GameManager.Instance.StartCoroutine(WaitForCooldown(agent.BasicAttackCooldown));
        }
        return;
    }

    //called when leavin stae
    public override void Exit(Spider agent)
    {
        return;
    }

    private IEnumerator WaitForCooldown(float cooldownTime)
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;

        yield break;
    }
}