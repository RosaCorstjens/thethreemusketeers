using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour
{
    UISprite highlight;

    public virtual void Initialize()
    {
        highlight = transform.FindChild("Highlight").GetComponent<UISprite>();
    }

    public virtual void HighlightButton()
    {
        highlight.gameObject.SetActive(true);
    }

    public virtual void DeHighlightButton()
    {
        highlight.gameObject.SetActive(false);
    }
}
