using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : State<EnemyController>
{
    //called once when entering state
    public override void Enter(EnemyController agent)
    {
        return;
    }

    //called every frame
    public override void Execute(EnemyController agent)
    {
        if (!agent.OnHitCooldown)
        {
            float distance = (agent.targetTransform.transform.position - agent.transform.position).magnitude;
            float direction = Vector3.Dot((agent.targetTransform.position - agent.transform.position).normalized,
                agent.transform.forward);

            if (distance < agent.NoticeDistance && distance > agent.AttackDistance)
            {
                agent.Rotate();

                agent.Move();
            }
            else if (distance <= agent.AttackDistance && direction > 0)
            {
                agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Attack"]);
            }
            else
            {
                agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Idle"]);
            }
        }
        else
        {
            agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["RecievedDamage"]);
        }
        return;
    }

    //called when leaving state
    public override void Exit(EnemyController agent)
    {
        return;
    }
}