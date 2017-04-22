using UnityEngine;
using System.Collections;

public class HUDBar : MonoBehaviour
{
    UISprite foregroundSprite;
    float totalWidth;

    public void Initialize()
    {
        foregroundSprite = transform.FindChild("Foreground").GetComponent<UISprite>();
        totalWidth = transform.GetComponent<UIWidget>().width;
    }

    public void SetBar(float currentValue, float maxValue)
    {
        foregroundSprite.width = (int)((totalWidth) / (maxValue/currentValue));
    }
}
