using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{
    TriggerArea trigger;
    Coroutine checkForEnter;

    public void Initialze()
    {
        //trigger = GetComponentInChildren<TriggerArea>();

        //trigger.onTriggerAction = PlayerInRange;
        //trigger.offTriggerAction = PlayerOutOfRange;
    }

    public void PlayerInRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.ShowPressToEnter(this.gameObject);

        checkForEnter = StartCoroutine(CheckForEnteringPortal());
    }

    public void PlayerOutOfRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.HideLabel();

        if (checkForEnter != null) StopCoroutine(checkForEnter);
    }

    private IEnumerator CheckForEnteringPortal()
    {
        while (!Input.GetKey(KeyCode.E))
        {
            yield return null;
        }

        checkForEnter = null;

        trigger.gameObject.SetActive(false);

        GameManager.Instance.UIManager.WorldUIManager.HideLabel();

        EnteredPortal();

        yield break;
    }

    private void EnteredPortal()
    {
        GameManager.Instance.LevelManager.DungeonManager.FinishDungeon();
    }
}
