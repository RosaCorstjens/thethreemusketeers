public class EnemyRecievedDamageState : State<Spider>
{
    float time;

    //called once when entering state
    public override void Enter(Spider agent)
    {
        time = 0;
    }

    //called every frame
    public override void Execute(Spider agent)
    {
        time += RealTime.deltaTime;
        if(time >= agent.CoolDownTime)
        {
            agent.FiniteStateMachine.GoBackState();
        }
        return;
    }

    //called when leavin stae
    public override void Exit(Spider agent)
    {
        agent.OnHitCooldown = false;
        return;
    }
}