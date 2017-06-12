using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    TriggerArea trigger;
    public Transform spike;
    Coroutine activateSpikes;
    bool dir;
    public float offset;


    void Awake()
    {
        trigger = GetComponent<TriggerArea>();

        spike = transform.FindChild("[trap]-spikes");

        trigger.onTriggerAction = PlayerInRange;
        trigger.offTriggerAction = PlayerOutOfRange;
    }

    public void PlayerInRange()
    {
        activateSpikes = StartCoroutine(MoveSpikes());
    }

    public void PlayerOutOfRange()
    {
        if (activateSpikes != null) StopCoroutine(activateSpikes);
    }

    public IEnumerator MoveSpikes()
    {
        while (true)
        {
            if (dir)
            {
                offset += Time.deltaTime;
            }
            else
            {
                offset -= Time.deltaTime * 0.2f;
            }

            if (offset >= 0)
            {
                offset = 0;
                dir = false;
            }
            else if (offset < -0.4)
            {
                dir = true;
                yield return new WaitForSeconds(2f);
            }

            spike.localPosition = new Vector3(0, 0, offset);

            yield return null;
        }

        yield break;
    }
}
