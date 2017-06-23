using UnityEngine;
using System.Collections;

public class HUDBar : MonoBehaviour
{
    private UISprite foregroundSprite;
    private UILabel healthNumbersLabel;
    private float totalWidth;

    public void Initialize()
    {
        foregroundSprite = transform.FindChild("Foreground").GetComponent<UISprite>();
        healthNumbersLabel = transform.FindChild("HealthIndicator").GetComponent<UILabel>();

        totalWidth = foregroundSprite.GetComponent<UIWidget>().width;
    }

    public void SetBar(float currentValue, float maxValue)
    {
        foregroundSprite.width = (int)((totalWidth) / (maxValue/currentValue));
        healthNumbersLabel.text = currentValue.ToString("F2") + "/" + maxValue.ToString("F2");
    }
}
