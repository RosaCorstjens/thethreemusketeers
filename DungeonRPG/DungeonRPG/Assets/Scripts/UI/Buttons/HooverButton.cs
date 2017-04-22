using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HooverButton : MonoBehaviour {

    private bool selected;
    public bool Selected { get { return selected; } set { selected = value; label.color = selected ? selectedColor : normalColor; } }
    UITweener tweener;
    UILabel label;
    public List<EventDelegate> del;
    Color normalColor;
    Color selectedColor;

    public void Initlialize()
    {
        selected = false;
        tweener = gameObject.GetComponent<UITweener>();
        label = gameObject.GetComponent<UILabel>();

        normalColor = label.color;
        selectedColor = Color.white;
    }

    public void HooverOn()
    {
        if (selected) return;
        tweener.PlayForward();
    }

    public void HooverOff()
    {
        if (selected) return;
        tweener.PlayReverse();
    }

    public void Click()
    {
        EventDelegate.Execute(del);
    }
}
