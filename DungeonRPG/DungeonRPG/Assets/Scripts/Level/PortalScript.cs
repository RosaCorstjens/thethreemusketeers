using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{
    TriggerArea trigger;
    Coroutine checkForEnter;

    public void Initialze()
    {
        trigger = GetComponent<TriggerArea>();

        trigger.onTriggerAction = PlayerInRange;
        trigger.offTriggerAction = PlayerOutOfRange;
    }

    public void PlayerInRange()
    {
        UIManager.Instance.WorldUIManager.ShowPressToEnter(this.gameObject);

        checkForEnter = StartCoroutine(CheckForEnteringPortal());
    }

    public void PlayerOutOfRange()
    {
        UIManager.Instance.WorldUIManager.HideLabel();

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

        UIManager.Instance.WorldUIManager.HideLabel();

        EnteredPortal();

        yield break;
    }

    private void EnteredPortal()
    {
        DungeonManager.Instance.FinishDungeon();
    }
}
