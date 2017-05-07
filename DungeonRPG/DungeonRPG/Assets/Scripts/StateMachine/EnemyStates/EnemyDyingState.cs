public class EnemyDyingState : State<EnemyController>
{
    //called once when entering state
    public override void Enter(EnemyController agent)
    {
        agent.Anim.SetBool("Dead", true);

        GameManager.Instance.ActiveCharacterInformation.AddExperiencePoints(agent.BaseProgression);

        GameManager.Instance.ActiveCharacterInformation.PlayerController.AdjustCurrentHealth(GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerKill));

        agent.Die();
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