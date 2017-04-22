using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoubleClick : MonoBehaviour
{
    public float delayBetween2Clicks; // Change value in editor
    private float lastClickTime = 0;

    public List<EventDelegate> onClick = new List<EventDelegate>();
    public List<EventDelegate> onDoubleClick = new List<EventDelegate>();
    public List<EventDelegate> onRightClick = new List<EventDelegate>();

    public void OnClickCallBack()
    {
        if (Time.time - lastClickTime < delayBetween2Clicks)
        {
            EventDelegate.Execute(onDoubleClick);
            lastClickTime = Time.time;
        }
        else
        {
            if (UICamera.currentTouchID == -1)
            {
                StartCoroutine(OnClickCoroutine());
                lastClickTime = Time.time;
            }
            else if (UICamera.currentTouchID == -2)
            {
                EventDelegate.Execute(onRightClick);
            }
        }
    }

    IEnumerator OnClickCoroutine()
    {
        yield return new WaitForSeconds(delayBetween2Clicks);

        if (Time.time - lastClickTime < delayBetween2Clicks)
        {
            yield break;
        }

        EventDelegate.Execute(onClick);
    }

}