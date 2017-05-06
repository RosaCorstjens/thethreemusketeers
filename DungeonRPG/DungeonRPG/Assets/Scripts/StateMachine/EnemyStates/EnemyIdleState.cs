public class EnemyIdleState : State<EnemyController>
{
    //called once when entering state
    public override void Enter(EnemyController agent)
    {
        agent.Anim.SetFloat("MoveZ", 0f);
        return;
    }

    //called every frame
    public override void Execute(EnemyController agent)
    {
        float distance = (agent.TargetTransform.transform.position - agent.transform.position).magnitude;
        if (distance < agent.NoticeDistance)
        {
            agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Battle"]);
        }
        return;
    }

    //called when leavin stae
    public override void Exit(EnemyController agent)
    {
        return;
    }
}