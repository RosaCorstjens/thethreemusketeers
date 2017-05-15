using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : State<Spider>
{
    //called once when entering state
    public override void Enter(Spider agent)
    {
        return;
    }

    //called every frame
    public override void Execute(Spider agent)
    {
        float distance = (agent.targetTransform.transform.position - agent.transform.position).magnitude;
        float direction = Vector3.Dot((agent.targetTransform.position - agent.transform.position).normalized,
            agent.transform.forward);

        if (distance < agent.NoticeDistance && distance > agent.AttackRange)
        {
            agent.Rotate();

            agent.Move();
        }
        else if (distance <= agent.AttackRange && direction > 0)
        {
            agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Attack"]);
        }
        else
        {
            agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Idle"]);
        }
        return;
    }

    //called when leaving state
    public override void Exit(Spider agent)
    {
        return;
    }
}