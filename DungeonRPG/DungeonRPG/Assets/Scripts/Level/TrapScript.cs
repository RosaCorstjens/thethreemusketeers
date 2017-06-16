using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] Transform spike;
    Coroutine moveSpikeCoroutine;

    bool spikesOut;
    float spikeSpeed = 1.5f;

    bool goingUp;
    [SerializeField] float yOffset;

    public void Initialize()
    {
        spike = transform.FindChild("[trap]-spikes");
        goingUp = true;
        yOffset = spike.localPosition.z;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (!spikesOut)
            {
                if(moveSpikeCoroutine != null) StopCoroutine(moveSpikeCoroutine);

                spikesOut = true;
                moveSpikeCoroutine = StartCoroutine(MoveSpikes());
            }
        }
    }

    public IEnumerator MoveSpikes()
    {
        yield return new WaitForSeconds(1f);

        while (spikesOut)
        {
            if (goingUp) { yOffset += spikeSpeed * Time.deltaTime; }
            else { yOffset -= spikeSpeed * 0.5f * Time.deltaTime; }

            if (yOffset >= 0) {
                yOffset = 0;
                goingUp = false;
                yield return new WaitForSeconds(0.5f);
            }
            else if (yOffset < -0.4) {
                spikesOut = false;
                goingUp = true;
            }

            spike.localPosition = new Vector3(0, 0, yOffset);           // since model is rotated 90 degrees local z axis is world y axis. 

            yield return null;
        }

        yield break;
    }
}
