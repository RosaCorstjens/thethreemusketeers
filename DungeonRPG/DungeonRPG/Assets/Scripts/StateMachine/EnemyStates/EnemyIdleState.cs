public class EnemyIdleState : State<Spider>
{
    //called once when entering state
    public override void Enter(Spider agent)
    {
        agent.Anim.SetFloat("MoveZ", 0f);
        return;
    }

    //called every frame
    public override void Execute(Spider agent)
    {
        float distance = (agent.targetTransform.transform.position - agent.transform.position).magnitude;
        if (distance < agent.NoticeDistance)
        {
            agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Battle"]);
        }
        return;
    }

    //called when leavin stae
    public override void Exit(Spider agent)
    {
        return;
    }
}