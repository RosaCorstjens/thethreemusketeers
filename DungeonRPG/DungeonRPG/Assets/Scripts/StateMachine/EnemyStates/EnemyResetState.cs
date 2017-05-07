public class EnemyResetSate : State<EnemyController>
{
    //called once when entering state
    public override void Enter(EnemyController agent)
    {
        agent.gameObject.SetActive(true);

        agent.Anim.SetTrigger("Revive");
        agent.Anim.SetBool("Dead", false);

        agent.transform.position = agent.startPosition;

        agent.targetTransform = GameManager.Instance.ActiveCharacter.transform;
        agent.targetScript = agent.targetTransform.gameObject.GetComponent<PlayerController>();

        //set to the default state
        agent.FiniteStateMachine.SetState(agent.FiniteStateMachine.PossibleStates["Idle"]);
        return;
    }

    //called every frame
    public override void Execute(EnemyController agent)
    {
        return;
    }

    //called when leavin stae
    public override void Exit(EnemyController agent)
    {
        return;
    }
}