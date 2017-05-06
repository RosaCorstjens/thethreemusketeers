public class EnemyRecievedDamageState : State<EnemyController>
{
    float time;

    //called once when entering state
    public override void Enter(EnemyController agent)
    {
        time = 0;
    }

    //called every frame
    public override void Execute(EnemyController agent)
    {
        time += RealTime.deltaTime;
        if(time >= agent.CoolDownTime)
        {
            agent.FiniteStateMachine.GoBackState();
        }
        return;
    }

    //called when leavin stae
    public override void Exit(EnemyController agent)
    {
        agent.OnHitCooldown = false;
        return;
    }
}