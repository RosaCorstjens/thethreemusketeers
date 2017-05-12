using System.Collections;
using UnityEngine;

public class EnemyDyingState : State<Spider>
{
    private const int beforeDeadTime = 2;

    //called once when entering state
    public override void Enter(Spider agent)
    {
        GameManager.Instance.StartCoroutine(WaitForCooldown(agent));
        return;
    }

    //called every frame
    public override void Execute(Spider agent)
    {
        return;
    }

    //called when leavin stae
    public override void Exit(Spider agent)
    {
        return;
    }


    private IEnumerator WaitForCooldown(Spider agent)
    {
        agent.Anim.SetBool("Dead", true);

        yield return new WaitForSeconds(beforeDeadTime);

        GameManager.Instance.ActiveCharacterInformation.AddExperiencePoints(agent.BaseProgression);

        GameManager.Instance.ActiveCharacterInformation.PlayerController.AdjustCurrentHealth(GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerKill));

        agent.Die();

        yield break;
    }

}