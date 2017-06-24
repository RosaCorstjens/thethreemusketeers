using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaMiniMapManager
{
    private GameObject megaMiniMapGameObject;                             // Go of the inventory panel. 
    private UIPanel megaMiniMapPanel;
    public Transform MegaMiniMapTransform { get { return megaMiniMapGameObject.transform; } }

    private Coroutine fadeIn;
    private Coroutine fadeOut;
    private bool fadingIn;
    private bool fadingOut;
    private float fadeTime = 0.5f;

    public void Initialize()
    {
        InstantiateMegaMiniMap();
        megaMiniMapGameObject.SetActive(false);
    }

    private void InstantiateMegaMiniMap()
    {
        // Get the inventory prefab and instantiate it. 
        megaMiniMapGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/MegaMiniMap")) as GameObject;
        megaMiniMapGameObject.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        megaMiniMapGameObject.transform.localScale = Vector3.one;

        megaMiniMapPanel = megaMiniMapGameObject.GetComponent<UIPanel>();
    }

    public IEnumerator FadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            if (fadeIn != null) GameManager.Instance.StopCoroutine(fadeIn);

            float startAlpha = megaMiniMapPanel.alpha;
            float rate = 1 / fadeTime;
            float progress = 0.0f;

            while (progress < 1)
            {
                megaMiniMapPanel.alpha = Mathf.Lerp(startAlpha, 0, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            megaMiniMapPanel.alpha = 0;
            fadingOut = false;
            megaMiniMapGameObject.SetActive(false);
        }
    }

    public IEnumerator FadeIn()
    {
        if (!fadingIn)
        {
            fadingIn = true;
            fadingOut = false;
            if (fadeOut != null) GameManager.Instance.StopCoroutine(fadeOut);

            float startAlpha = megaMiniMapPanel.alpha;
            float rate = 1 / fadeTime;
            float progress = 0.0f;

            megaMiniMapGameObject.SetActive(true);

            while (progress < 1)
            {
                megaMiniMapPanel.alpha = Mathf.Lerp(startAlpha, 1, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }

            megaMiniMapPanel.alpha = 1;
            fadingIn = false;
        }
    }

    public void ToggleMenu(bool on)
    {
        if (on) fadeIn = GameManager.Instance.StartCoroutine(FadeIn());
        else fadeOut = GameManager.Instance.StartCoroutine(FadeOut());
    }

}
